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
    public class CarPartComponentDescsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarPartComponentDescs
        public async Task<ActionResult> Index()
        {
            var carPartComponentDescs = db.CarPartComponentDescs
                .Include(c => c.CarPartComponent)
                .Include(c => c.CarPartComponent.CarPart)
                .Include(c => c.CarPartComponent.CarPart.Car)
                .Include(c => c.CarPartComponent.CarPart.Car.CarBrand);
            return View(await carPartComponentDescs.ToListAsync());
        }

        // GET: CarPartComponentDescs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            if (carPartComponentDesc == null)
            {
                return HttpNotFound();
            }
            return View(carPartComponentDesc);
        }

        // GET: CarPartComponentDescs/Create
        public ActionResult Create()
        {
            ViewBag.CarPartComponentId = new SelectList(db.CarPartComponents.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.CarPart.Car.CarBrand.Name + " " + c.CarPart.Car.Name + " "+ c.CarPart.Name  + ")" }), "Id", "Name");
            return View();
        }

        // POST: CarPartComponentDescs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CarPartComponentId,CreatedAt")] CarPartComponentDesc carPartComponentDesc)
        {
            if (ModelState.IsValid)
            {
                db.CarPartComponentDescs.Add(carPartComponentDesc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CarPartComponentId = new SelectList(db.CarPartComponents.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.CarPart.Car.CarBrand.Name + " " + c.CarPart.Car.Name + " " + c.CarPart.Name + ")" }), "Id", "Name", carPartComponentDesc.CarPartComponentId);
            return View(carPartComponentDesc);
        }

        // GET: CarPartComponentDescs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            if (carPartComponentDesc == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarPartComponentId = new SelectList(db.CarPartComponents.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.CarPart.Car.CarBrand.Name + " " + c.CarPart.Car.Name + " " + c.CarPart.Name + ")" }), "Id", "Name", carPartComponentDesc.CarPartComponentId);
            return View(carPartComponentDesc);
        }

        // POST: CarPartComponentDescs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CarPartComponentId,CreatedAt")] CarPartComponentDesc carPartComponentDesc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carPartComponentDesc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarPartComponentId = new SelectList(db.CarPartComponents.Select(c => new { Id = c.Id, Name = c.Name + " (" + c.CarPart.Car.CarBrand.Name + " " + c.CarPart.Car.Name + " " + c.CarPart.Name + ")" }), "Id", "Name", carPartComponentDesc.CarPartComponentId);
            return View(carPartComponentDesc);
        }

        // GET: CarPartComponentDescs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            if (carPartComponentDesc == null)
            {
                return HttpNotFound();
            }
            return View(carPartComponentDesc);
        }

        // POST: CarPartComponentDescs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            db.CarPartComponentDescs.Remove(carPartComponentDesc);
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
