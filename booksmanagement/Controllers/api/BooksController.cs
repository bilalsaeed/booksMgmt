using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using booksmanagement.Dtos;
using booksmanagement.Helpers;
using booksmanagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using tusdotnet.Stores;

namespace booksmanagement.Controllers.api
{
    public class BooksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Books
        public IQueryable<Book> GetBooks()
        {
            return db.Books
                .Include(b => b.Car)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.CarPartComponentDesc);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SearchBooks([FromBody] BookRequestSearchDto searchDto)
        {
            var books = await db.Books
                .Where(b =>
                        (searchDto.FromDate == null || b.CreatedDate >= searchDto.FromDate) &&
                        (searchDto.ToDate == null || b.CreatedDate <= searchDto.ToDate)
                    )
                .Include(b => b.Car)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.CarPartComponentDesc).ToListAsync();

            return Ok(books);
        }

        public IQueryable<Book> GetSoftBooks()
        {
            return db.Books
                .Include(b => b.Car)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.CarPartComponentDesc)
                .Where(b => b.TypeId == 1 && b.IsActive);
        }
        public IQueryable<Book> GetHardBooks()
        {
            return db.Books
                .Include(b => b.Car)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.CarPartComponentDesc)
                .Where(b => b.TypeId == 2 && b.IsActive);
        }

        public IQueryable<AppUserDto> GetAllDrawerUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var drawerRole = roleManager.FindByName(RoleName.Drawer);

            return db.Users.Where(u => u.IsActive && u.Roles.Any(s => s.RoleId == drawerRole.Id)).Select(u => new AppUserDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FirstName + " " + u.LastName
            });
        }

        // GET: api/Books/5
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetBookSoftFiles(int id)
        {
            var fileObj = await db.BookMediaFiles.Where(b => b.BookId == id && b.Type == "S")
                .Select(b => new { Id= b.Id, FileName= b.FileName }).ToListAsync();
            
            return Ok(fileObj);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public async Task<IHttpActionResult> PutBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (book.Id == 0)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }

                if (ex.InnerException.InnerException.Message.ToLower().Contains("cannot insert duplicate key row in object"))
                    return BadRequest("Book already exists for this");
                else
                    return BadRequest(ex.Message);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var softBookId = 0;
                var hardBookId = 0;
                if (book.TypeId == 3)
                {
                    var softBook = new Book();
                    softBook.TypeId = 1;
                    softBook.Title = book.Title;
                    softBook.Description = book.Description;
                    softBook.Keywords = book.Keywords;
                    softBook.CarId = book.CarId;
                    softBook.CarPartId = book.CarPartId;
                    softBook.CarPartComponentId = book.CarPartComponentId;
                    softBook.CarPartComponentDescId = book.CarPartComponentDescId;
                    softBook.BookLocation = book.BookLocation;
                    softBook.BookNumber = book.BookNumber;
                    softBook.PartCodeAvailable = book.PartCodeAvailable;
                    softBook.SoftCopyAvailable = book.SoftCopyAvailable;
                    softBook.IsActive = book.IsActive;
                    softBook.DownloadLink = book.DownloadLink;
                    db.Books.Add(softBook);

                    var hardBook = new Book();
                    hardBook.TypeId = 2;
                    hardBook.Title = book.Title;
                    hardBook.Description = book.Description;
                    hardBook.Keywords = book.Keywords;
                    hardBook.CarId = book.CarId;
                    hardBook.CarPartId = book.CarPartId;
                    hardBook.CarPartComponentId = book.CarPartComponentId;
                    hardBook.CarPartComponentDescId = book.CarPartComponentDescId;
                    hardBook.BookLocation = book.BookLocation;
                    hardBook.BookNumber = book.BookNumber;
                    hardBook.PartCodeAvailable = book.PartCodeAvailable;
                    hardBook.IsActive = book.IsActive;
                    hardBook.Quantity = book.Quantity;
                    db.Books.Add(hardBook);

                    await db.SaveChangesAsync();

                    softBookId = softBook.Id;
                    hardBookId = hardBook.Id;
                }
                else if (book.TypeId == 1 || book.TypeId == 2)
                {
                    db.Books.Add(book);

                    await db.SaveChangesAsync();
                }

                return CreatedAtRoute("DefaultApi", new { id = book.Id, softId = softBookId, hardId = hardBookId }, new { id = book.Id, softId = softBookId, hardId = hardBookId });
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.ToLower().Contains("cannot insert duplicate key row in object"))
                    return BadRequest("Book already exists for this");
                else
                    return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }

        //[System.Web.Http.HttpPost]
        //public async Task<IHttpActionResult> PostBookMediaFiles([FromBody] BookMediaFiles[] mediaFiles)
        //{
        //    try
        //    {
        //        if (mediaFiles != null && mediaFiles.Length > 0)
        //        {
        //            foreach (BookMediaFiles drawing in mediaFiles)
        //            {
        //                var existing = await db.BookMediaFiles.Where(d => d.BookId == drawing.BookId && d.Type == drawing.Type).ToListAsync();
        //                db.BookMediaFiles.RemoveRange(existing);
        //            }

        //            db.BookMediaFiles.AddRange(mediaFiles);
        //            await db.SaveChangesAsync();
        //            return Ok(new { success = true });
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/Books/GetSoftFile/{id}")]
        //public async Task<HttpResponseMessage> GetSoftFile(int id)
        //{
        //    try
        //    {
        //        var fileObj = await db.BookMediaFiles.Where(a => a.BookId == id && a.Type == "S").FirstOrDefaultAsync();
        //        var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
        //        var file = await store.GetFileAsync(fileObj.FileId, CancellationToken.None);

        //        if (file == null)
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NotFound);
        //        }
        //        var fileStream = await file.GetContentAsync(CancellationToken.None);
        //        var metadata = await file.GetMetadataAsync(CancellationToken.None);

        //        // The tus protocol does not specify any required metadata.
        //        // "filetype" is metadata that is specific to this domain and is not required.
        //        var type = metadata.ContainsKey("filetype")
        //                  ? metadata["filetype"].GetString(Encoding.UTF8)
        //                  : "application/octet-stream";

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await fileStream.CopyToAsync(memoryStream);
        //            fileStream.Close();
        //            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(memoryStream.ToArray())
        //            };
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

        //            return response;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //}

        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("api/Books/GetPartCodeFile/{id}")]
        //public async Task<HttpResponseMessage> GetPartCodeFile(int id)
        //{
        //    try
        //    {
        //        var fileObj = await db.BookMediaFiles.Where(a => a.BookId == id && a.Type == "P").FirstOrDefaultAsync();
        //        var store = new TusDiskStore(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/tusfiles");
        //        var file = await store.GetFileAsync(fileObj.FileId, CancellationToken.None);

        //        if (file == null)
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.NotFound);
        //        }
        //        var fileStream = await file.GetContentAsync(CancellationToken.None);
        //        var metadata = await file.GetMetadataAsync(CancellationToken.None);

        //        // The tus protocol does not specify any required metadata.
        //        // "filetype" is metadata that is specific to this domain and is not required.
        //        var type = metadata.ContainsKey("filetype")
        //                  ? metadata["filetype"].GetString(Encoding.UTF8)
        //                  : "application/octet-stream";

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await fileStream.CopyToAsync(memoryStream);
        //            fileStream.Close();
        //            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ByteArrayContent(memoryStream.ToArray())
        //            };
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);

        //            return response;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //}
    }
}