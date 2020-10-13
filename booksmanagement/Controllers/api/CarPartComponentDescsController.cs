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
    public class CarPartComponentDescsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CarPartComponentDescs
        public IQueryable<CarPartComponentDesc> GetCarPartComponentDescs()
        {
            return db.CarPartComponentDescs
                .Include(c => c.CarPartComponent)
                .Include(c => c.CarPartComponent.CarPart)
                .Include(c => c.CarPartComponent.CarPart.Car)
                .Include(c => c.CarPartComponent.CarPart.Car.CarBrand);
        }

        // GET: api/CarPartComponentDescs/5
        [ResponseType(typeof(CarPartComponentDesc))]
        public async Task<IHttpActionResult> GetCarPartComponentDesc(int id)
        {
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            if (carPartComponentDesc == null)
            {
                return NotFound();
            }

            return Ok(carPartComponentDesc);
        }

        // PUT: api/CarPartComponentDescs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarPartComponentDesc(int id, CarPartComponentDesc carPartComponentDesc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carPartComponentDesc.Id)
            {
                return BadRequest();
            }

            db.Entry(carPartComponentDesc).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarPartComponentDescExists(id))
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

        // POST: api/CarPartComponentDescs
        [ResponseType(typeof(CarPartComponentDesc))]
        public async Task<IHttpActionResult> PostCarPartComponentDesc(CarPartComponentDesc carPartComponentDesc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CarPartComponentDescs.Add(carPartComponentDesc);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carPartComponentDesc.Id }, carPartComponentDesc);
        }

        // DELETE: api/CarPartComponentDescs/5
        [ResponseType(typeof(CarPartComponentDesc))]
        public async Task<IHttpActionResult> DeleteCarPartComponentDesc(int id)
        {
            CarPartComponentDesc carPartComponentDesc = await db.CarPartComponentDescs.FindAsync(id);
            if (carPartComponentDesc == null)
            {
                return NotFound();
            }

            db.CarPartComponentDescs.Remove(carPartComponentDesc);
            await db.SaveChangesAsync();

            return Ok(carPartComponentDesc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarPartComponentDescExists(int id)
        {
            return db.CarPartComponentDescs.Count(e => e.Id == id) > 0;
        }
    }
}