﻿using System;
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

namespace booksmanagement.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class CarPartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarParts
        public async Task<ActionResult> Index()
        {
            var carParts = db.CarParts.Include(c => c.Car);
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
            return View();
        }

        // POST: CarParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CarId,CreatedAt")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {
                db.CarParts.Add(carPart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", carPart.CarId);
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
            return View(carPart);
        }

        // POST: CarParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CarId,CreatedAt")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carPart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Name", carPart.CarId);
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
