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
    public class BookBorrowController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> GetPendingRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<BookBorrowOrder> requests = null;

            if (User.IsInRole(RoleName.Admin))
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Include(b => b.Applicant)
                .Where(b => b.Status == 1).OrderBy(b => b.Status).ToListAsync();
            }
            else
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Where(b => b.ApplicantId == appUser.Id && b.Status == 1).ToListAsync();
            }

            return Ok(requests);
        }

        //[HttpGet]
        //public async Task<IHttpActionResult> GetMyAllRequests()
        //{
        //    var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();



        //    return Ok(requests);
        //}

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IHttpActionResult> GetOtherPendingRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Where(b => b.Status == 1).ToListAsync();

            return Ok(requests);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IHttpActionResult> GetOtherAllRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .OrderBy(b => b.Status).ToListAsync();

            return Ok(requests);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveBookRequest(BookRequestDto bookRequest)
        {
            if (bookRequest == null)
                return BadRequest();

            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var book = db.Books.Where(b => b.Id == bookRequest.BookId).FirstOrDefault();
            if (book == null)
                return BadRequest();

            if (book.Quantity == null || book.Quantity <= 0 || !book.IsActive)
                return BadRequest("Book is not available");

            BookBorrowOrder order = new BookBorrowOrder()
            {
                BookId = bookRequest.BookId,
                Purpose = bookRequest.Purpose,
                Status = 1, //this is a pending request
                FromDate = bookRequest.FromDate,
                ToDate = bookRequest.ToDate,
                ApplicantId = appUser.Id,
            };

            db.BookBorrowOrders.Add(order);
            //await db.SaveChangesAsync();


            book.Quantity -= 1;
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
