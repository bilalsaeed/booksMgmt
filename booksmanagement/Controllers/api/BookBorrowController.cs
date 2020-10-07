using booksmanagement.Dtos;
using booksmanagement.Models;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        public async Task<IHttpActionResult> SaveBookRequest(BookRequestDto bookRequest)
        {
            if (bookRequest == null)
                return BadRequest();

            var appUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var book = db.Books.Where(b => b.Id == bookRequest.BookId).FirstOrDefault();
            if(book == null)
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
