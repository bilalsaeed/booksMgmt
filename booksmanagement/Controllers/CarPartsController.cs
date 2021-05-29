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
    public class CarPartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarParts
        public async Task<ActionResult> Index()
        {
            var carParts = db.CarParts.Include(c => c.Car).Include(c => c.CarPartType);
            return View(await carParts.ToListAsync());
        }

        // GET: CarParts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPart carPart = await db.CarParts.FindAsync(id);
            if (carPart == null)
            {
                return HttpNotFound();
            }
            return View(carPart);
        }

        // GET: CarParts/Create
        public ActionResult Create()
        {
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name");
            ViewBag.CarPartTypeId = new SelectList(db.CarPartTypes, "Id", "Name");
            return View();
        }

        // POST: CarParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CarId,CarPartTypeId,CreatedAt")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {

                db.CarParts.Add(carPart);
                await db.SaveChangesAsync();

                if (Request.Files.Count > 0)
                {
                    //var file = Request.Files[0];
                    if (Request.Files.Count > 0)
                    {
                        var files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            var file = files[i];

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
                                drawingFile.CarPartId = carPart.Id;
                                db.DrawingFiles.Add(drawingFile);

                                carPart.IsDrawingAvailable = true;
                            }
                        }
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", carPart.CarId);
            ViewBag.CarPartTypeId = new SelectList(db.CarPartTypes, "Id", "Name", carPart.CarPartTypeId);
            return View(carPart);
        }

        // GET: CarParts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPart carPart = await db.CarParts.FindAsync(id);
            if (carPart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", carPart.CarId);
            ViewBag.CarPartTypeId = new SelectList(db.CarPartTypes, "Id", "Name", carPart.CarPartTypeId);
            return View(carPart);
        }

        // POST: CarParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CarId,CarPartTypeId,CreatedAt,IsDrawingAvailable")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    bool drawingsFound = false;
                    var oldDrawings = db.DrawingFiles.Where(d => d.CarPartId == carPart.Id).ToList();
                    var files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            drawingsFound = true;
                            string fileName = file.FileName;
                            string fileExtension = file.ContentType;

                            BinaryReader b = new BinaryReader(file.InputStream);

                            DrawingFiles drawingFile = new DrawingFiles();
                            drawingFile.File = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));
                            drawingFile.FileName = fileName;
                            drawingFile.FileType = fileExtension;
                            drawingFile.Type = "P";
                            drawingFile.FileSize = file.ContentLength;
                            drawingFile.CarPartId = carPart.Id;
                            db.DrawingFiles.Add(drawingFile);

                            carPart.IsDrawingAvailable = true;

                        }
                    }
                    if (drawingsFound)
                        db.DrawingFiles.RemoveRange(oldDrawings);
                }
                db.Entry(carPart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", carPart.CarId);
            ViewBag.CarPartTypeId = new SelectList(db.CarPartTypes, "Id", "Name", carPart.CarPartTypeId);
            return View(carPart);
        }

        // GET: CarParts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPart carPart = await db.CarParts.FindAsync(id);
            if (carPart == null)
            {
                return HttpNotFound();
            }
            return View(carPart);
        }

        // POST: CarParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CarPart carPart = await db.CarParts.FindAsync(id);
            db.CarParts.Remove(carPart);
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
