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
        public async Task<IHttpActionResult> GetCarTree(bool drawingAvailable)
        {

            CarTreeDto carTree = new CarTreeDto()
            {
                CarBrands = db.CarBrands.OrderBy(b => b.Name)
                .Select(b => new CarBrandDto()

                {
                    Id = b.Id,
                    Name = b.Name,
                    collapsed = true,
                    childerns = db.Cars.OrderBy(x => x.Name)
                    .Where(c => c.CarBrandId == b.Id
                    && (!drawingAvailable || db.CarParts.Where(cp => cp.CarId == c.Id
                         && (drawingAvailable ||
                             db.CarPartComponents
                             .Where(cmp => cmp.CarPartId == cp.Id && !cmp.IsDrawingAvailable)
                             .Count() > 0 || !cp.IsDrawingAvailable)
                         && (!drawingAvailable ||
                             db.CarPartComponents
                             .Where(cmp => cmp.CarPartId == cp.Id && cmp.IsDrawingAvailable)
                             .Count() > 0 || cp.IsDrawingAvailable))
                        .Count() > 0
                        || c.IsDrawingAvailable)
                    && (drawingAvailable || db.CarParts.Where(cp => cp.CarId == c.Id
                         && (drawingAvailable ||
                             db.CarPartComponents
                             .Where(cmp => cmp.CarPartId == cp.Id && !cmp.IsDrawingAvailable)
                             .Count() > 0 || !cp.IsDrawingAvailable)
                         && (!drawingAvailable ||
                             db.CarPartComponents
                             .Where(cmp => cmp.CarPartId == cp.Id && cmp.IsDrawingAvailable)
                             .Count() > 0 || cp.IsDrawingAvailable))
                        .Count() > 0
                        || !c.IsDrawingAvailable)
                        )
                    .Select(c => new CarDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Class = c.Class,
                        collapsed = true,
                        car = true,
                        DrawingOrderId = c.DrawingOrderId,
                        IsDrawingAvailable = c.IsDrawingAvailable,
                        childerns = db.CarPartTypes.OrderBy(x => x.Name).Where(ct => db.CarParts
                        .Where(cp => cp.CarId == c.Id && (drawingAvailable ||
                            db.CarPartComponents
                            .Where(cmp => cmp.CarPartId == cp.Id && !cmp.IsDrawingAvailable)
                            .Count() > 0 || !cp.IsDrawingAvailable)
                        && (!drawingAvailable ||
                            db.CarPartComponents
                            .Where(cmp => cmp.CarPartId == cp.Id && cmp.IsDrawingAvailable)
                            .Count() > 0 || cp.IsDrawingAvailable))
                        .Select(x => x.CarPartTypeId).ToList().Contains(ct.Id)).Select(ct => new CarPartTypeDto
                        {
                            Id = ct.Id,
                            Name = ct.Name,
                            collapsed = true,
                            childerns = db.CarParts.OrderBy(x => x.Name)
                            .Where(cp => cp.CarPartTypeId == ct.Id && cp.CarId == c.Id
                                   && (drawingAvailable || db.CarPartComponents.Where(cmp => cmp.CarPartId == cp.Id && !cmp.IsDrawingAvailable).Count() > 0 || !cp.IsDrawingAvailable)
                                   && (!drawingAvailable || db.CarPartComponents.Where(cmp => cmp.CarPartId == cp.Id && cmp.IsDrawingAvailable).Count() > 0 || cp.IsDrawingAvailable))
                            .Select(cp => new CarPartDto
                            {
                                Id = cp.Id,
                                Name = cp.Name,
                                CarId = c.Id,
                                CarPartTypeId = cp.CarPartTypeId,
                                CarPartType = cp.CarPartType,
                                collapsed = true,
                                carPart = true,
                                DrawingOrderId = cp.DrawingOrderId,
                                IsDrawingAvailable = cp.IsDrawingAvailable,
                                childerns = db.CarPartComponents.OrderBy(x => x.Name)
                                .Where(comp => comp.CarPartId == cp.Id
                                        && (drawingAvailable || !comp.IsDrawingAvailable)
                                        && (!drawingAvailable || comp.IsDrawingAvailable))
                                .Select(comp => new CarPartComponentDto
                                {
                                    Id = comp.Id,
                                    Name = comp.Name,
                                    CarId = c.Id,
                                    CarPartId = comp.CarPartId,
                                    carPartComp = true,
                                    DrawingOrderId = comp.DrawingOrderId,
                                    IsDrawingAvailable = comp.IsDrawingAvailable
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            //CarTreeDto carTree = new CarTreeDto() { 
            //    CarBrands = new CarBrandDto() { },

            //};

            //Cars = db.Cars.ToList(),
            //    CarParts = db.CarParts.Include(p => p.CarPartType).ToList(),
            //    CarPartComponents = db.CarPartComponents.ToList()



            return Ok(carTree);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetCarBookTree(bool archive)
        {
            CarTreeBookDto carTree = new CarTreeBookDto()
            {
                CarBrands = db.CarBrands.OrderBy(b => b.Name).Select(b => new CarBrandBookDto()
                {
                    Id = b.Id,
                    Name = b.Name,
                    collapsed = true,
                    childerns = db.Cars.OrderBy(x => x.Name).Where(c => c.CarBrandId == b.Id && c.IsArchived == archive).Select(c => new CarBookDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Class = c.Class,
                        collapsed = true,
                        car = true,
                        bookAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && (bk.TypeId == 2 && bk.Quantity != 0) && bk.CarPartId == null).Count() != 0,
                        softCopy = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.TypeId == 1 && bk.CarPartId == null).Count() != 0,
                        partCodeAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == null).Select(bk => bk.PartCodeAvailable).FirstOrDefault(),
                        softCopyAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == null && bk.TypeId == 1).Select(bk => bk.SoftCopyAvailable).FirstOrDefault(),
                        maintainancePlanAvailable = c.MaintenancePlan == null ? false : true,
                        bookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == null && bk.TypeId == 2).Select(bk => bk.Id).FirstOrDefault(),
                        softBookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == null && bk.TypeId == 1).Select(bk => bk.Id).FirstOrDefault(),
                        maintainancePlanId = c.MaintenancePlanId,
                        childerns = db.CarParts.OrderBy(x => x.Name).Where(cp => cp.CarId == c.Id).Select(cp => new CarPartDto
                        {
                            Id = cp.Id,
                            Name = cp.Name,
                            CarPartTypeId = cp.CarPartTypeId,
                            CarPartType = cp.CarPartType,
                            collapsed = true,
                            carPart = true,
                            CarId = cp.CarId,
                            bookAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && (bk.TypeId == 2 && bk.Quantity != 0) && bk.CarPartId == cp.Id && bk.CarPartComponentId == null).Count() != 0,
                            softCopy = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.TypeId == 1 && bk.CarPartId == cp.Id && bk.CarPartComponentId == null).Count() != 0,
                            partCodeAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == null).Select(bk => bk.PartCodeAvailable).FirstOrDefault(),
                            softCopyAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == null && bk.TypeId == 1).Select(bk => bk.SoftCopyAvailable).FirstOrDefault(),
                            bookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == null && bk.TypeId == 2).Select(bk => bk.Id).FirstOrDefault(),
                            softBookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == null && bk.TypeId == 1).Select(bk => bk.Id).FirstOrDefault(),
                            childerns = db.CarPartComponents.OrderBy(x => x.Name).Where(comp => comp.CarPartId == cp.Id).Select(comp => new CarPartComponentDto
                            {
                                Id = comp.Id,
                                Name = comp.Name,
                                CarPartId = comp.CarPartId,
                                carPartComp = true,
                                collapsed = true,
                                bookAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && (bk.TypeId == 2 && bk.Quantity != 0) && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null).Count() != 0,
                                softCopy = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.TypeId == 1 && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null).Count() != 0,
                                partCodeAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null).Select(bk => bk.PartCodeAvailable).FirstOrDefault(),
                                softCopyAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null && bk.TypeId == 1).Select(bk => bk.SoftCopyAvailable).FirstOrDefault(),
                                bookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null && bk.TypeId == 2).Select(bk => bk.Id).FirstOrDefault(),
                                softBookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == null && bk.TypeId == 1).Select(bk => bk.Id).FirstOrDefault(),
                                childerns = db.CarPartComponentDescs.OrderBy(x => x.Name).Where(cmpDe => cmpDe.CarPartComponentId == comp.Id).Select(cmpDe => new CarPartComponentDescDto
                                {
                                    Id = cmpDe.Id,
                                    Name = cmpDe.Name,
                                    CarPartComponentId = cmpDe.CarPartComponentId,
                                    carPartCompDesc = true,
                                    bookAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && (bk.TypeId == 2 && bk.Quantity != 0) && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id).Count() != 0,
                                    softCopy = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.TypeId == 1 && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id).Count() != 0,
                                    partCodeAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id).Select(bk => bk.PartCodeAvailable).FirstOrDefault(),
                                    softCopyAvailable = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id && bk.TypeId == 1).Select(bk => bk.SoftCopyAvailable).FirstOrDefault(),
                                    bookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id && bk.TypeId == 2).Select(bk => bk.Id).FirstOrDefault(),
                                    softBookId = db.Books.Where(bk => bk.CarId == c.Id && bk.IsActive && bk.CarPartId == cp.Id && bk.CarPartComponentId == comp.Id && bk.CarPartComponentDescId == cmpDe.Id && bk.TypeId == 1).Select(bk => bk.Id).FirstOrDefault()
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList()
            };


            return Ok(carTree);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllDrawingOrders()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<DrawingOrder> orders = null;

            if (User.IsInRole(RoleName.Admin))
            {
                orders = await db.DrawingOrders
                .Include(b => b.Car)
                .Include(b => b.Car.CarBrand)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.Applicant)
                .Include(b => b.AssignedTo)
                //.Where(b => b.AssignedTo == null).OrderBy(b => b.AppliedDate)
                .ToListAsync();
            }
            else
            {
                orders = await db.DrawingOrders
                .Include(b => b.Car)
                .Include(b => b.Car.CarBrand)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.AssignedTo)
                .Where(b => b.ApplicantId == appUser.Id || b.AssignedToId == appUser.Id).ToListAsync();
            }

            return Ok(orders);
        }

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SearchAllDrawings([FromBody] BookRequestSearchDto searchDto)
        {
            var orders = await db.DrawingOrders
                .Where(d =>
                        (searchDto.FromDate == null || d.AppliedDate >= searchDto.FromDate) &&
                        (searchDto.ToDate == null || d.AppliedDate <= searchDto.ToDate)
                    )
                .Include(b => b.Car)
                .Include(b => b.Car.CarBrand)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.AssignedTo)
                .Include(b => b.Applicant)
                .ToListAsync();

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
                .Include(b => b.Car)
                .Include(b => b.Car.CarBrand)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.Applicant)
                .Where(b => b.AssignedTo == null).OrderBy(b => b.AppliedDate)
                .ToListAsync();
            }
            else
            {
                orders = await db.DrawingOrders
                .Include(b => b.Car)
                .Include(b => b.Car.CarBrand)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.AssignedTo)
                .Where(b => (b.ApplicantId == appUser.Id && !b.IsApproved) || (b.AssignedToId == appUser.Id && !b.IsApproved && !b.DrawingSubmitted)).ToListAsync();
            }

            return Ok(orders);
        }


        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SaveDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            var car = await db.Cars.Where(p => p.Id == orderRequest.CarId).FirstOrDefaultAsync();
            if (car == null)
                return BadRequest();


            DrawingOrder drawingOrder = new DrawingOrder()
            {
                CarId = car.Id,
                CarPartId = orderRequest.CarPartId,
                CarPartComponentId = orderRequest.CarPartComponentId,
                Description = orderRequest.Description,
                Purpose = orderRequest.Purpose,
                Location = orderRequest.Location,
                JobNumber = orderRequest.JobNumber,
                ApplicantId = appUser?.Id
            };

            db.DrawingOrders.Add(drawingOrder);

            try
            {
                await db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            var oldFiles = db.DrawingFiles.Where(m => m.DrawingOrderId == drawingOrder.Id && m.Type == "P").ToList();
            db.DrawingFiles.RemoveRange(oldFiles);


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
                    foreach (DrawingFiles drawing in mediaFiles)
                    {
                        var existing = await db.DrawingFiles.Where(d => d.DrawingOrderId == drawing.DrawingOrderId).ToListAsync();
                        db.DrawingFiles.RemoveRange(existing);
                    }

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

            var drawingOrder = db.DrawingOrders.Find(id);
            List<DrawingFiles> drawingFiles = new List<DrawingFiles>();
            if (drawingOrder.CarPartComponentId != null || drawingOrder.CarPartComponentId != 0)
                drawingFiles = await db.DrawingFiles.Where(a => a.CarPartComponentId == drawingOrder.CarPartComponentId).ToListAsync();
            else if (drawingOrder.CarPartId != null || drawingOrder.CarPartId != 0)
                drawingFiles = await db.DrawingFiles.Where(a => a.CarPartId == drawingOrder.CarPartId).ToListAsync();
            else if (drawingOrder.CarId != null || drawingOrder.CarId != 0)
                drawingFiles = await db.DrawingFiles.Where(a => a.CarId == drawingOrder.CarId).ToListAsync();

            var address = $"{request.Url.Scheme}://{request.Url.Host}:{request.Url.Port}{appUrl}";
            //var files = await db.DrawingFiles.Where(a => a.DrawingOrderId == id).ToListAsync();
            var list = from k in drawingFiles
                       select new
                       {
                           k.Id,
                           k.FileType,
                           k.DrawingOrderId,
                           k.FileName,
                           k.FileSize,
                           k.Type,
                           thumbUrl = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFileThumbnail&&FileId={k.Id}&&width=100&&height=100",
                           url = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFile&&FileId={k.Id}"

                       };

            return Ok(list);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetCarDrawingFiles(int id)
        {
            var request = HttpContext.Current.Request;

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            //if (appUrl != "/")
            //    appUrl = "/" + appUrl;


            var address = $"{request.Url.Scheme}://{request.Url.Host}:{request.Url.Port}{appUrl}";
            var files = await db.DrawingFiles.Where(a => a.CarId == id).ToListAsync();
            var list = from k in files
                       select new
                       {
                           k.Id,
                           k.FileType,
                           k.DrawingOrderId,
                           k.FileName,
                           k.FileSize,
                           k.Type,
                           thumbUrl = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFileThumbnail&&FileId={k.Id}&&width=100&&height=100",
                           url = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFile&&FileId={k.Id}"

                       };

            return Ok(list);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetCarPartDrawingFiles(int id)
        {
            var request = HttpContext.Current.Request;

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            //if (appUrl != "/")
            //    appUrl = "/" + appUrl;


            var address = $"{request.Url.Scheme}://{request.Url.Host}:{request.Url.Port}{appUrl}";
            var files = await db.DrawingFiles.Where(a => a.CarPartId == id)
                .Select(a => new
                {
                    a.Id,
                    a.FileType,
                    a.DrawingOrderId,
                    a.FileName,
                    a.FileSize,
                    a.Type
                }).ToListAsync();
            var list = from k in files
                       select new
                       {
                           k.Id,
                           k.FileType,
                           k.DrawingOrderId,
                           k.FileName,
                           k.FileSize,
                           k.Type,
                           thumbUrl = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFileThumbnail&&FileId={k.Id}&&width=100&&height=100",
                           url = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFile&&FileId={k.Id}"

                       };

            return Ok(list);
        }
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetCarPartCompDrawingFiles(int id)
        {
            var request = HttpContext.Current.Request;

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            //if (appUrl != "/")
            //    appUrl = "/" + appUrl;


            var address = $"{request.Url.Scheme}://{request.Url.Host}:{request.Url.Port}{appUrl}";
            var files = await db.DrawingFiles.Where(a => a.CarPartComponentId == id)
                .Select(a => new
                {
                    a.Id,
                    a.FileType,
                    a.DrawingOrderId,
                    a.FileName,
                    a.FileSize,
                    a.Type
                }).ToListAsync();
            var list = from k in files
                       select new
                       {
                           k.Id,
                           k.FileType,
                           k.DrawingOrderId,
                           k.FileName,
                           k.FileSize,
                           k.Type,
                           thumbUrl = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFileThumbnail&&FileId={k.Id}&&width=100&&height=100",
                           url = $"{address}/HttpHandlers/FileRequestHandler.ashx?Type=GetDrawingFile&&FileId={k.Id}"

                       };

            return Ok(list);
        }

        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/DrawingOrders/GetFileThumbnail/{id}/{width}/{height}")]
        //public async Task<HttpResponseMessage> GetFileThumbnail(string id, int width = 348, int height = 218)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        try
        //        {
        //            var fileObj = await db.DrawingFiles.Where(a => a.FileId == id).FirstOrDefaultAsync();
        //            var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
        //            var file = await store.GetFileAsync(id, CancellationToken.None);

        //            if (file == null)
        //            {
        //                return new HttpResponseMessage(HttpStatusCode.NotFound);
        //            }
        //            var fileStream = await file.GetContentAsync(CancellationToken.None);
        //            var metadata = await file.GetMetadataAsync(CancellationToken.None);

        //            // The tus protocol does not specify any required metadata.
        //            // "filetype" is metadata that is specific to this domain and is not required.
        //            var type = metadata.ContainsKey("filetype")
        //                      ? metadata["filetype"].GetString(Encoding.UTF8)
        //                      : "application/octet-stream";
        //            using (Image imgPhoto = Image.Load(fileStream))
        //            {

        //                ResizeOptions resizeOpt = new ResizeOptions()
        //                {
        //                    // Mode = ResizeMode.Min,
        //                    Size = new Size() { Height = height, Width = width }
        //                };
        //                imgPhoto.Mutate(x => x
        //                     .Resize(resizeOpt)
        //                 );
        //                MemoryStream ms = new MemoryStream();
        //                await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
        //                fileStream.Close();
        //                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        //                {
        //                    Content = new ByteArrayContent(ms.ToArray())
        //                };
        //                //response.Content.Headers.ContentDisposition =
        //                //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = fileObj?.Name };
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NotFound);
        //        }
        //        // }
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.NotFound);
        //}
        //[System.Web.Http.HttpGet]
        //public async Task<HttpResponseMessage> GetFile(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        try
        //        {
        //            var fileObj = await db.DrawingFiles.Where(a => a.FileId == id).FirstOrDefaultAsync();
        //            var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
        //            var file = await store.GetFileAsync(id, CancellationToken.None);

        //            if (file == null)
        //            {
        //                return new HttpResponseMessage(HttpStatusCode.NotFound);
        //            }
        //            var fileStream = await file.GetContentAsync(CancellationToken.None);
        //            var metadata = await file.GetMetadataAsync(CancellationToken.None);

        //            // The tus protocol does not specify any required metadata.
        //            // "filetype" is metadata that is specific to this domain and is not required.
        //            var type = metadata.ContainsKey("filetype")
        //                      ? metadata["filetype"].GetString(Encoding.UTF8)
        //                      : "application/octet-stream";
        //            //using (Image imgPhoto = Image.Load(fileStream))
        //            //{

        //            //    ResizeOptions resizeOpt = new ResizeOptions()
        //            //    {
        //            //        // Mode = ResizeMode.Min,
        //            //        Size = new Size() { Height = height, Width = width }
        //            //    };
        //            //    imgPhoto.Mutate(x => x
        //            //         .Resize(resizeOpt)
        //            //     );
        //            //    MemoryStream ms = new MemoryStream();
        //            //    await imgPhoto.SaveAsPngAsync(ms); // Automatic encoder selected based on extension.
        //            //    fileStream.Close();

        //            //}
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                await fileStream.CopyToAsync(memoryStream);
        //                fileStream.Close();
        //                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        //                {
        //                    Content = new ByteArrayContent(memoryStream.ToArray())
        //                };
        //                //response.Content.Headers.ContentDisposition =
        //                //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = fileObj?.Name };
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

        //                return response;
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NotFound);
        //        }
        //        // }
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.NotFound);
        //}




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

            if (drawingOrder.CarPartComponentId != null || drawingOrder.CarPartComponentId != 0)
            {
                var drawingsCount = db.DrawingFiles.Where(d => d.CarPartComponentId == drawingOrder.CarPartComponentId).Count();
                if (drawingsCount > 0)
                {
                    var carPartComp = db.CarPartComponents.Where(p => p.Id == drawingOrder.CarPartComponentId).FirstOrDefault();
                    if (carPartComp != null)
                        carPartComp.IsDrawingAvailable = true;
                }
            }
            else if (drawingOrder.CarPartId != null || drawingOrder.CarPartId != 0)
            {
                var drawingsCount = db.DrawingFiles.Where(d => d.CarPartId == drawingOrder.CarPartId).Count();
                if (drawingsCount > 0)
                {
                    var carPart = db.CarParts.Where(p => p.Id == drawingOrder.CarPartId).FirstOrDefault();
                    if (carPart != null)
                        carPart.IsDrawingAvailable = true;
                }
            }
            else if (drawingOrder.CarId != null || drawingOrder.CarId != 0)
            {
                var drawingsCount = db.DrawingFiles.Where(d => d.CarId == drawingOrder.CarId).Count();
                if (drawingsCount > 0)
                {
                    var car = db.Cars.Where(p => p.Id == drawingOrder.CarId).FirstOrDefault();
                    if (car != null)
                        car.IsDrawingAvailable = true;
                }
            }


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


            drawingOrder.RejectionComments = orderRequest.RejectionComments;
            drawingOrder.DrawingRejected = true;
            drawingOrder.DrawingRejectedDate = DateTime.UtcNow;
            drawingOrder.DrawingSubmitted = false;
            drawingOrder.DrawingSubmittedDate = null;


            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
