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
    public class CarPartsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CarParts
        public IQueryable<CarPart> GetCarParts()
        {
            return db.CarParts.Include(c => c.Car).Include(c => c.Car.CarBrand);
        }


        // GET: api/CarParts/5
        [ResponseType(typeof(CarPart))]
        public async Task<IHttpActionResult> GetCarPart(int id)
        {
            CarPart carPart = await db.CarParts.FindAsync(id);
            if (carPart == null)
            {
                return NotFound();
            }

            return Ok(carPart);
        }

        // PUT: api/CarParts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarPart(int id, CarPart carPart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carPart.Id)
            {
                return BadRequest();
            }

            db.Entry(carPart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarPartExists(id))
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

        // POST: api/CarParts
        [ResponseType(typeof(CarPart))]
        public async Task<IHttpActionResult> PostCarPart(CarPart carPart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarParts.Add(carPart);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carPart.Id }, carPart);
        }

        // DELETE: api/CarParts/5
        [ResponseType(typeof(CarPart))]
        public async Task<IHttpActionResult> DeleteCarPart(int id)
        {
            CarPart carPart = await db.CarParts.FindAsync(id);
            if (carPart == null)
            {
                return NotFound();
            }

            db.CarParts.Remove(carPart);
            await db.SaveChangesAsync();

            return Ok(carPart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarPartExists(int id)
        {
            return db.CarParts.Count(e => e.Id == id) > 0;
        }
    }
}