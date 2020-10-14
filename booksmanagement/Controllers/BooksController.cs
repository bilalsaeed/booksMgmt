using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using booksmanagement.Models;

namespace booksmanagement.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var books = db.Books
                .Include(b => b.Car)
                .Include(b => b.CarPart)
                .Include(b => b.CarPartComponent)
                .Include(b => b.CarPartComponentDesc);
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            //ViewBag.CarId = new SelectList(db.Cars, "Id", "Name");
            //ViewBag.CarPartId = new SelectList(db.CarParts.Select(c => new { c.Id, Name = c.Name + " (" + c.Car.CarBrand.Name + " " + c.Car.Name + ")" }), "Id", "Name");
            //ViewBag.CarPartComponentId = new SelectList(db.CarPartComponents.Select(c => new { c.Id, Name = c.Name + " (" + c.CarPart.Car.CarBrand.Name + " " + c.CarPart.Car.Name + " " + c.CarPart.Name + ")" }), "Id", "Name");
            //ViewBag.CarPartComponentDescId = new SelectList(db.CarPartComponentDescs.Select(c => new { c.Id, Name = c.Name + " ("+ c.CarPartComponent.CarPart.Car.CarBrand.Name + " " + c.CarPartComponent.CarPart.Car.Name + " " + c.CarPartComponent.CarPart.Name + ")" }), "Id", "Name");

            //ViewBag.TypeId = new List<SelectListItem>
            //{
            //    new SelectListItem{ Text="Soft copy", Value = "1", Selected= true },
            //    new SelectListItem{ Text="Hard copy", Value = "2" },
            //};

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Keywords,DownloadLink,CarId,CarPartId,CarPartComponentId,CarPartComponentDescId,TypeId,Quantity,IsActive,CreatedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", book.CarId);
            ViewBag.TypeId = new List<SelectListItem>
            {
                new SelectListItem{ Text="Soft copy", Value = "1", Selected= true },
                new SelectListItem{ Text="Hard copy", Value = "2" },
            };
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", book.CarId);
            ViewBag.TypeId = new SelectList(new[]
            {
                new { Id = "1", Name = "Soft copy" },
                new { Id = "2", Name = "Hard copy" }
            }, "Id", "Name", book.TypeId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Keywords,DownloadLink,CarId,TypeId,Quantity,IsActive,CreatedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", book.CarId);
            ViewBag.TypeId = new List<SelectListItem>
            {
                new SelectListItem{ Text="Soft copy", Value = "1" },
                new SelectListItem{ Text="Hard copy", Value = "2" },
            };
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await db.Books.FindAsync(id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
