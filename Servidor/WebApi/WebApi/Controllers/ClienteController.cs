using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ClienteController : ApiController
    {
        private GasStationPharmacyEntities db = new GasStationPharmacyEntities();

        [Route("api/Cliente/{cedula}/{contrasena}")]
        public HttpResponseMessage Get(int cedula, string contrasena)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cliente = RepositorioCliente.GetCliente(cedula, contrasena);
            HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound, cliente);
            if (cliente == null)
            {
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, cliente);
            return response;
        }
    }
}
