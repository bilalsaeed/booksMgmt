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
using booksmanagement.Helpers;
using System.IO;

namespace booksmanagement.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class CarPartComponentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarPartComponents
        public async Task<ActionResult> Index()
        {
            var carPartComponents = db.CarPartComponents
                .Include(c => c.CarPart)
                .Include(c => c.CarPart.Car)
                .Include(c => c.CarPart.Car.CarBrand);
            return View(await carPartComponents.ToListAsync());
        }

        // GET: CarPartComponents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            if (carPartComponent == null)
            {
                return HttpNotFound();
            }
            return View(carPartComponent);
        }

        // GET: CarPartComponents/Create
        public ActionResult Create()
        {
            ViewBag.CarPartId = new SelectList(db.CarParts.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.Car.CarBrand.Name + " " + c.Car.Name + ")" }), "Id", "Name");
            return View();
        }

        // POST: CarPartComponents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CarPartId,CreatedAt")] CarPartComponent carPartComponent)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;

                        BinaryReader b = new BinaryReader(file.InputStream);

                        DrawingFiles drawingFile = new DrawingFiles();
                        drawingFile.File = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));
                        drawingFile.FileName = fileName;
                        drawingFile.FileType = fileExtension;
                        drawingFile.Type = "P";
                        drawingFile.FileSize = file.ContentLength;
                        db.DrawingFiles.Add(drawingFile);
                        db.SaveChanges();

                        carPartComponent.DrawingFilesId = drawingFile.Id;
                    }
                }

                db.CarPartComponents.Add(carPartComponent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CarPartId = new SelectList(db.CarParts.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.Car.CarBrand.Name + " " + c.Car.Name + ")" }), "Id", "Name", carPartComponent.CarPartId);
            return View(carPartComponent);
        }

        // GET: CarPartComponents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            if (carPartComponent == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarPartId = new SelectList(db.CarParts.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.Car.CarBrand.Name + " " + c.Car.Name + ")" }), "Id", "Name", carPartComponent.CarPartId);
            return View(carPartComponent);
        }

        // POST: CarPartComponents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CarPartId,CreatedAt")] CarPartComponent carPartComponent)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;

                        BinaryReader b = new BinaryReader(file.InputStream);

                        DrawingFiles drawingFile = new DrawingFiles();
                        drawingFile.File = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));
                        drawingFile.FileName = fileName;
                        drawingFile.FileType = fileExtension;
                        drawingFile.Type = "P";
                        drawingFile.FileSize = file.ContentLength;
                        db.DrawingFiles.Add(drawingFile);
                        db.SaveChanges();

                        carPartComponent.DrawingFilesId = drawingFile.Id;
                    }
                }

                db.Entry(carPartComponent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarPartId = new SelectList(db.CarParts.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.Car.CarBrand.Name + " " + c.Car.Name + ")" }), "Id", "Name", carPartComponent.CarPartId);
            return View(carPartComponent);
        }

        // GET: CarPartComponents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            if (carPartComponent == null)
            {
                return HttpNotFound();
            }
            return View(carPartComponent);
        }

        // POST: CarPartComponents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            db.CarPartComponents.Remove(carPartComponent);
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
