using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using booksmanagement.Models;

namespace booksmanagement.Controllers.api
{
    public class CarPartComponentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CarPartComponents
        public IQueryable<CarPartComponent> GetCarPartComponents()
        {
            return db.CarPartComponents
                .Include(c => c.CarPart)
                .Include(c => c.CarPart.Car)
                .Include(c => c.CarPart.Car.CarBrand);
        }

        public IQueryable<CarPartComponent> GetCarPartComponentsByPart(int partId)
        {
            return db.CarPartComponents
                .Include(c => c.CarPart)
                .Include(c => c.CarPart.Car)
                .Include(c => c.CarPart.Car.CarBrand)
                .Where(c => c.CarPartId == partId);
        }

        // GET: api/CarPartComponents/5
        [ResponseType(typeof(CarPartComponent))]
        public async Task<IHttpActionResult> GetCarPartComponent(int id)
        {
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            if (carPartComponent == null)
            {
                return NotFound();
            }

            return Ok(carPartComponent);
        }

        // PUT: api/CarPartComponents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarPartComponent(int id, CarPartComponent carPartComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carPartComponent.Id)
            {
                return BadRequest();
            }

            db.Entry(carPartComponent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarPartComponentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CarPartComponents
        [ResponseType(typeof(CarPartComponent))]
        public async Task<IHttpActionResult> PostCarPartComponent(CarPartComponent carPartComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarPartComponents.Add(carPartComponent);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carPartComponent.Id }, carPartComponent);
        }

        // DELETE: api/CarPartComponents/5
        [ResponseType(typeof(CarPartComponent))]
        public async Task<IHttpActionResult> DeleteCarPartComponent(int id)
        {
            CarPartComponent carPartComponent = await db.CarPartComponents.FindAsync(id);
            if (carPartComponent == null)
            {
                return NotFound();
            }

            db.CarPartComponents.Remove(carPartComponent);
            await db.SaveChangesAsync();

            return Ok(carPartComponent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarPartComponentExists(int id)
        {
            return db.CarPartComponents.Count(e => e.Id == id) > 0;
        }
    }
}