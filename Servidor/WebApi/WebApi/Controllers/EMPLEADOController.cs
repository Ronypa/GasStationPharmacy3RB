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
    
    public class EmpleadoController : ApiController
    {
        private GasStationPharmacyEntities db = new GasStationPharmacyEntities();
        
        [Route("api/Empleado/{cedula}/{contrasena}")]
        public HttpResponseMessage Get(int cedula, string contrasena)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var empleado = RepositorioEmpleado.GetEmpleado(cedula,contrasena);
            HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound, empleado);
            if (empleado == null)
            {
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, empleado);
            return response;
        }

    }
}