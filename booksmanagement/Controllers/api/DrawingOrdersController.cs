using booksmanagement.Dtos;
using booksmanagement.Helpers;
using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace booksmanagement.Controllers.api
{
    public class DrawingOrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
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

        [HttpGet]
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


        [HttpPost]
        public async Task<IHttpActionResult> SaveDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var carPart = db.CarParts.Where(p => p.Id == orderRequest.CarPartId).FirstOrDefault();
            if (carPart == null)
                return BadRequest();


            DrawingOrder drawingOrder = new DrawingOrder()
            {
                CarPartId = carPart.Id,
                CarPartComponentId = orderRequest.CarPartComponentId,
                Description = orderRequest.Description,
                Purpose = orderRequest.Purpose,
                Location = orderRequest.Location,
                ApplicantId = appUser.Id
            };

            db.DrawingOrders.Add(drawingOrder);

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IHttpActionResult> SubmitDrawingOrder(DrawingOrderDto orderRequest)
        {
            if (orderRequest == null)
                return BadRequest();

            var appUser = await db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var drawingOrder = await db.DrawingOrders.Where(d => d.Id == orderRequest.Id).FirstOrDefaultAsync();
            if (drawingOrder == null)
                return BadRequest("Drawing order is not found");

            
            if (drawingOrder.AssignedToId != appUser.Id)
            {
                return BadRequest("User is not authorized for this operations");
            }



            drawingOrder.DrawingSubmitted = true;
            drawingOrder.DrawingSubmittedDate = DateTime.UtcNow;


            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
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

        [HttpPost]
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
