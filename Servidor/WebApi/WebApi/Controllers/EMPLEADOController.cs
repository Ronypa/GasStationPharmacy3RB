using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EMPLEADOController : ApiController
    {
        private GasStationPharmacyEntities db = new GasStationPharmacyEntities();

        // GET: api/EMPLEADO
        public IQueryable<EMPLEADO> GetEMPLEADOes()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.EMPLEADOes;
        }

        // GET: api/EMPLEADO/5
        [ResponseType(typeof(EMPLEADO))]
        public IHttpActionResult GetEMPLEADO(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            EMPLEADO eMPLEADO = db.EMPLEADOes.Find(id);
            if (eMPLEADO == null)
            {
                return NotFound();
            }

            return Ok(eMPLEADO);
        }

        // PUT: api/EMPLEADO/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEMPLEADO(int id, EMPLEADO eMPLEADO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eMPLEADO.Cedula)
            {
                return BadRequest();
            }

            db.Entry(eMPLEADO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EMPLEADOExists(id))
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

        // POST: api/EMPLEADO
        [ResponseType(typeof(EMPLEADO))]
        public IHttpActionResult PostEMPLEADO(EMPLEADO eMPLEADO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EMPLEADOes.Add(eMPLEADO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EMPLEADOExists(eMPLEADO.Cedula))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = eMPLEADO.Cedula }, eMPLEADO);
        }

        // DELETE: api/EMPLEADO/5
        [ResponseType(typeof(EMPLEADO))]
        public IHttpActionResult DeleteEMPLEADO(int id)
        {
            EMPLEADO eMPLEADO = db.EMPLEADOes.Find(id);
            if (eMPLEADO == null)
            {
                return NotFound();
            }

            db.EMPLEADOes.Remove(eMPLEADO);
            db.SaveChanges();

            return Ok(eMPLEADO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EMPLEADOExists(int id)
        {
            return db.EMPLEADOes.Count(e => e.Cedula == id) > 0;
        }
    }
}