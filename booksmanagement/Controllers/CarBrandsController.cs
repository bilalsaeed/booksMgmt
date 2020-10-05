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
    public class CarBrandsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarBrands
        public async Task<ActionResult> Index()
        {
            return View(await db.CarBrands.ToListAsync());
        }

        // GET: CarBrands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return HttpNotFound();
            }
            return View(carBrand);
        }

        // GET: CarBrands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarBrands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CreatedAt")] CarBrand carBrand)
        {
            if (ModelState.IsValid)
            {
                db.CarBrands.Add(carBrand);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(carBrand);
        }

        // GET: CarBrands/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return HttpNotFound();
            }
            return View(carBrand);
        }

        // POST: CarBrands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreatedAt")] CarBrand carBrand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carBrand).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(carBrand);
        }

        // GET: CarBrands/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return HttpNotFound();
            }
            return View(carBrand);
        }

        // POST: CarBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            db.CarBrands.Remove(carBrand);
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
