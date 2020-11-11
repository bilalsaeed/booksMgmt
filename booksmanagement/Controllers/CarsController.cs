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
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index()
        {
            var cars = db.Cars.Include(c => c.CarBrand);
            return View(await cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.CarBrandId = new SelectList(db.CarBrands, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CarBrandId,Class,CreatedAt,IsArchived")] Car car)
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

                        GeneralMediaFile mediaFile = new GeneralMediaFile();
                        mediaFile.File = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));
                        mediaFile.FileName = fileName;
                        mediaFile.FileType = fileExtension;
                        //mediaFile.Type = "P";
                        mediaFile.FileSize = file.ContentLength;
                        db.GeneralMediaFiles.Add(mediaFile);
                        db.SaveChanges();

                        car.MaintenancePlanId = mediaFile.Id;
                    }
                }

                db.Cars.Add(car);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CarBrandId = new SelectList(db.CarBrands, "Id", "Name", car.CarBrandId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarBrandId = new SelectList(db.CarBrands, "Id", "Name", car.CarBrandId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CarBrandId,Class,CreatedAt,IsArchived")] Car car)
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

                        GeneralMediaFile mediaFile = new GeneralMediaFile();
                        mediaFile.File = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));
                        mediaFile.FileName = fileName;
                        mediaFile.FileType = fileExtension;
                        //mediaFile.Type = "P";
                        mediaFile.FileSize = file.ContentLength;
                        db.GeneralMediaFiles.Add(mediaFile);
                        db.SaveChanges();

                        if(car.MaintenancePlanId != null)
                        {
                            var oldFile = db.GeneralMediaFiles.Where(f => f.Id == car.MaintenancePlanId).FirstOrDefault();
                            if (oldFile != null)
                                db.GeneralMediaFiles.Remove(oldFile);
                        }

                        car.MaintenancePlanId = mediaFile.Id;
                    }
                }

                db.Entry(car).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarBrandId = new SelectList(db.CarBrands, "Id", "Name", car.CarBrandId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Car car = await db.Cars.FindAsync(id);
            db.Cars.Remove(car);
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
