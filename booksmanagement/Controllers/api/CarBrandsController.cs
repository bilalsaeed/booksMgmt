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
    public class CarBrandsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CarBrands
        public IQueryable<CarBrand> GetCarBrands()
        {
            return db.CarBrands;
        }

        // GET: api/CarBrands/5
        [ResponseType(typeof(CarBrand))]
        public async Task<IHttpActionResult> GetCarBrand(int id)
        {
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return NotFound();
            }

            return Ok(carBrand);
        }

        // PUT: api/CarBrands/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarBrand(int id, CarBrand carBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carBrand.Id)
            {
                return BadRequest();
            }

            db.Entry(carBrand).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarBrandExists(id))
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

        // POST: api/CarBrands
        [ResponseType(typeof(CarBrand))]
        public async Task<IHttpActionResult> PostCarBrand(CarBrand carBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarBrands.Add(carBrand);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carBrand.Id }, carBrand);
        }

        // DELETE: api/CarBrands/5
        [ResponseType(typeof(CarBrand))]
        public async Task<IHttpActionResult> DeleteCarBrand(int id)
        {
            CarBrand carBrand = await db.CarBrands.FindAsync(id);
            if (carBrand == null)
            {
                return NotFound();
            }

            db.CarBrands.Remove(carBrand);
            await db.SaveChangesAsync();

            return Ok(carBrand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarBrandExists(int id)
        {
            return db.CarBrands.Count(e => e.Id == id) > 0;
        }
    }
}