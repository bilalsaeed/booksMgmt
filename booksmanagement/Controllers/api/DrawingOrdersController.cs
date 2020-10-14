using booksmanagement.Dtos;
using booksmanagement.Helpers;
using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using System.Web;
using System.Web.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using tusdotnet.Stores;

namespace booksmanagement.Controllers.api
{
    public class DrawingOrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllDrawingOrders()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<DrawingOrder> orders = null;

            if (User.IsInRole(RoleName.Admin))
            {
                orders = await db.DrawingOrders
                .Include(b => b.CarPart)
                .Include(b => b.CarPart.Car)
                .Include(b => b.CarPart.Car.CarBrand)
                .Include(b => b.CarPartComponent)
                .Include(b => b.Applicant)
                .Include(b => b.AssignedTo)
                //.Where(b => b.AssignedTo == null).OrderBy(b => b.AppliedDate)
                .ToListAsync();
            }
            else
            {
                orders = await db.DrawingOrders
                .Include(b => b.CarPart)
                .Include(b => b.CarPart.Car)
                .Include(b => b.CarPart.Car.CarBrand)
                .Include(b => b.CarPartComponent)
                .Include(b => b.AssignedTo)
                .Where(b => b.ApplicantId == appUser.Id || b.AssignedToId == appUser.Id).ToListAsync();
            }

            return Ok(orders);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetPendingDrawingOrders()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<DrawingOrder> orders = null;

            if (User.IsInRole(RoleName.Admin))
            {
                orders = await db.DrawingOrders
                .Include(b => b.CarPart)
                .Include(b => b.Applicant)
                .Where(b => b.AssignedTo == null).OrderBy(b => b.AppliedDate)
                .ToListAsync();
            }
            else
            {
                orders = await db.DrawingOrders
                .Include(b => b.CarPart)
                .Where(b => b.ApplicantId == appUser.Id && !b.IsApproved).ToListAsync();
            }

            return Ok(orders);
        }


        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SaveDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            var carPart = await db.CarParts.Where(p => p.Id == orderRequest.CarPartId).FirstOrDefaultAsync();
            if (carPart == null)
                return BadRequest();


            DrawingOrder drawingOrder = new DrawingOrder()
            {
                CarPartId = carPart.Id,
                CarPartComponentId = orderRequest.CarPartComponentId,
                Description = orderRequest.Description,
                Purpose = orderRequest.Purpose,
                Location = orderRequest.Location,
                ApplicantId = appUser?.Id
            };

            db.DrawingOrders.Add(drawingOrder);

            await db.SaveChangesAsync();

            return Ok();
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AssignDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (!User.IsInRole(RoleName.Admin))
            {
                return BadRequest("User is not authorized for this operations");
            }


            var assignedUser = await db.Users.Where(p => p.Id == orderRequest.AssignedToId).FirstOrDefaultAsync();
            if (assignedUser == null)
                return BadRequest("Assgined user is not found.");

            var drawingOrder = await db.DrawingOrders.Where(d => d.Id == orderRequest.Id).FirstOrDefaultAsync();
            if (drawingOrder == null)
                return BadRequest("Drawing order is not found");

            drawingOrder.AssignedToId = assignedUser.Id;
            drawingOrder.AssignedDate = DateTime.UtcNow;


            await db.SaveChangesAsync();

            return Ok();
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SubmitDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var drawingOrder = await db.DrawingOrders.Where(d => d.Id == orderRequest.Id).FirstOrDefaultAsync();
            if (drawingOrder == null)
                return BadRequest("Drawing order is not found");


            if (drawingOrder.AssignedToId != appUser?.Id)
            {
                return BadRequest("User is not authorized for this operations");
            }



            drawingOrder.DrawingSubmitted = true;
            drawingOrder.DrawingSubmittedDate = DateTime.UtcNow;


            await db.SaveChangesAsync();

            return Ok();
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> PostDrawingMediaFiles([FromBody] DrawingFiles[] mediaFiles)
        {
            try
            {
                if (mediaFiles != null && mediaFiles.Length > 0)
                {
                    db.DrawingFiles.AddRange(mediaFiles);
                    await db.SaveChangesAsync();
                    return Ok(new { success = true });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetDrawingFiles(int id)
        {
            var request = HttpContext.Current.Request;

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            //if (appUrl != "/")
            //    appUrl = "/" + appUrl;

            var address = $"{request.Url.Scheme}://{request.Url.Host}{appUrl}";
            var files = await db.DrawingFiles.Where(a => a.DrawingOrderId == id).ToListAsync();
            var list = from k in files
                       select new
                       {
                           k.Id,
                           k.ContentType,
                           k.CreateAt,
                           k.DrawingOrderId,
                           k.FileId,
                           k.Name,
                           k.Size,
                           k.Type,
                           thumbUrl = $"{address}/api/DrawingOrders/GetFileThumbnail/{k.FileId}/100/100",
                           url = $"{address}/api/DrawingOrders/GetFileThumbnail/{k.FileId}"

                       };

            return Ok(list);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/DrawingOrders/GetFileThumbnail/{id}/{width}/{height}")]
        public async Task<HttpResponseMessage> GetFileThumbnail(string id, int width = 348, int height = 218)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var fileObj = await db.DrawingFiles.Where(a => a.FileId == id).FirstOrDefaultAsync();
                    var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
                    var file = await store.GetFileAsync(id, CancellationToken.None);

                    if (file == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                    var fileStream = await file.GetContentAsync(CancellationToken.None);
                    var metadata = await file.GetMetadataAsync(CancellationToken.None);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    using (Image imgPhoto = Image.Load(fileStream))
                    {

                        ResizeOptions resizeOpt = new ResizeOptions()
                        {
                            // Mode = ResizeMode.Min,
                            Size = new Size() { Height = height, Width = width }
                        };
                        imgPhoto.Mutate(x => x
                             .Resize(resizeOpt)
                         );
                        MemoryStream ms = new MemoryStream();
                        await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                        fileStream.Close();
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(ms.ToArray())
                        };
                        //response.Content.Headers.ContentDisposition =
                        //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = fileObj?.Name };
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                // }
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetFile(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var fileObj = await db.DrawingFiles.Where(a => a.FileId == id).FirstOrDefaultAsync();
                    var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
                    var file = await store.GetFileAsync(id, CancellationToken.None);

                    if (file == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                    var fileStream = await file.GetContentAsync(CancellationToken.None);
                    var metadata = await file.GetMetadataAsync(CancellationToken.None);

                    // The tus protocol does not specify any required metadata.
                    // "filetype" is metadata that is specific to this domain and is not required.
                    var type = metadata.ContainsKey("filetype")
                              ? metadata["filetype"].GetString(Encoding.UTF8)
                              : "application/octet-stream";
                    //using (Image imgPhoto = Image.Load(fileStream))
                    //{

                    //    ResizeOptions resizeOpt = new ResizeOptions()
                    //    {
                    //        // Mode = ResizeMode.Min,
                    //        Size = new Size() { Height = height, Width = width }
                    //    };
                    //    imgPhoto.Mutate(x => x
                    //         .Resize(resizeOpt)
                    //     );
                    //    MemoryStream ms = new MemoryStream();
                    //    await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
                    //    fileStream.Close();

                    //}
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        fileStream.Close();
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(memoryStream.ToArray())
                        };
                        //response.Content.Headers.ContentDisposition =
                        //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = fileObj?.Name };
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

                        return response;
                    }
                   
                    
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                // }
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }




        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ApproveDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var drawingOrder = await db.DrawingOrders.Where(d => d.Id == orderRequest.Id).FirstOrDefaultAsync();
            if (drawingOrder == null)
                return BadRequest("Drawing order is not found");


            if (drawingOrder.ApplicantId != appUser.Id)
            {
                return BadRequest("User is not authorized for this operations");
            }



            drawingOrder.IsApproved = true;
            drawingOrder.ApprovalDate = DateTime.UtcNow;


            await db.SaveChangesAsync();

            return Ok();
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> RejectDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var drawingOrder = await db.DrawingOrders.Where(d => d.Id == orderRequest.Id).FirstOrDefaultAsync();
            if (drawingOrder == null)
                return BadRequest("Drawing order is not found");


            if (drawingOrder.ApplicantId != appUser.Id)
            {
                return BadRequest("User is not authorized for this operations");
            }



            drawingOrder.DrawingRejected = true;
            drawingOrder.DrawingRejectedDate = DateTime.UtcNow;
            drawingOrder.DrawingSubmitted = false;
            drawingOrder.DrawingSubmittedDate = null;


            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
