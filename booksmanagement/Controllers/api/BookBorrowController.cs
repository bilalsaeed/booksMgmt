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

        [HttpGet]
        public async Task<IHttpActionResult> GetApprovedRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<BookBorrowOrder> requests = null;

            if (User.IsInRole(RoleName.Admin))
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Include(b => b.Applicant)
                .Where(b => b.Status == 2).OrderBy(b => b.Status).ToListAsync();
            }
            else
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Where(b => b.ApplicantId == appUser.Id && b.Status == 2).ToListAsync();
            }

            return Ok(requests);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetGrantedRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<BookBorrowOrder> requests = null;

            if (User.IsInRole(RoleName.Admin))
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Include(b => b.Applicant)
                .Where(b => b.Status == 3)
                .OrderBy(b => b.ToDate)
                .ToListAsync();
            }
            else
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Where(b => b.ApplicantId == appUser.Id && b.Status == 3)
                .OrderBy(b => b.ToDate)
                .ToListAsync();
            }

            return Ok(requests);
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetAllRequests()
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            List<BookBorrowOrder> requests = null;

            if (User.IsInRole(RoleName.Admin))
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .Include(b => b.Applicant)
                .OrderBy(b => b.Status)
                .ToListAsync();
            }
            else
            {
                requests = await db.BookBorrowOrders
                .Include(b => b.Book)
                .OrderBy(b => b.Status)
                .Where(b => b.ApplicantId == appUser.Id).ToListAsync();
            }

            return Ok(requests);
        }


        [HttpPost]
        public async Task<IHttpActionResult> ApproveBookRequest([FromBody] int requestId)
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!User.IsInRole(RoleName.Admin))
                return BadRequest("User is not authorized for this operation.");

            var request = await db.BookBorrowOrders.Where(b => b.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest("Request not found.");

            if(request.Status == 1)
            {
                request.Status = 2;
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> GrantBook([FromBody] int requestId)
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //if (!User.IsInRole(RoleName.Admin))
            //    return BadRequest("User is not authorized for this operation.");

            var request = await db.BookBorrowOrders.Where(b => b.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest("Request not found.");

            if (request.Status == 2)
            {
                request.Status = 3;
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> CollectBook([FromBody] int requestId)
        {
            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (!User.IsInRole(RoleName.Admin))
                return BadRequest("User is not authorized for this operation.");

            var request = await db.BookBorrowOrders.Where(b => b.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest("Request not found.");

            if (request.Status == 3)
            {
                request.Status = 4;
                var book = await db.Books.Where(b => b.Id == request.BookId).FirstOrDefaultAsync();
                book.Quantity += 1;

                await db.SaveChangesAsync();
            }

            return Ok();
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
