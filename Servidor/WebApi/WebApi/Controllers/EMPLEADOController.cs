using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    
    public class EMPLEADOController : ApiController
    {
        private GasStationPharmacyEntities db = new GasStationPharmacyEntities();
        
        // GET api/ptemployees/5
        [Route("api/Empleado/{id}/{pass}")]
        public HttpResponseMessage Get(int id, string pass)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var employees = RepositorioEmpleado.GetEmpleado(id,pass);
            HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound, employees);
            if (employees == null)
            {
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

    }
}