using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    /// <summary>
    /// Recibe peticiones para mostrar estadisticas
    /// </summary>
    public class EstadisticasController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Recibe la solicitud de consultar los productos mas vendidos por compañia
        /// </summary>
        /// <param name="obj">compañia a consultar</param>
        /// <returns>HTTP OK y el resultado de la estadistica</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("estadisticaProductosCompania")]
        public HttpResponseMessage estadisticaProductosCompania(ObjGeneral obj)
        {
            List<ObjGeneralEstadistica> estadisticas = 
                ProcesadorEstadisticas.ProcesoProductosCompania(obj.opcion);
            if (estadisticas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, estadisticas);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Recibe la solicitud de consultar la estadistica de ventas de la compañia
        /// </summary>
        /// <param name="obj">nombre de compañia a consultar</param>
        /// <returns>HTTP OK y el resultado de la estadistica</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("estadisticaVentas")]
        public HttpResponseMessage estadisticaVentas(ObjGeneral obj)
        {
            int estadistica =
                ProcesadorEstadisticas.ProcesoVentasTotales(obj.opcion);
            if (estadistica == -1)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, estadistica);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Recibe la solicitud de consultar la estadistica de productos en general
        /// </summary>
        /// <returns>HTPP ok informacion de la estadistica</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("estadisticaProductosTotal")]
        public HttpResponseMessage estadisticaProductosTotal()
        {
            List<ObjGeneralEstadistica> estadisticas =
                ProcesadorEstadisticas.ProcesoProductosTotales();
            if (estadisticas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, estadisticas);
            return response;
        }
    }
}