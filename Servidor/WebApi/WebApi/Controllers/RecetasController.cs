using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    public class RecetasController : ApiController
    {
        [HttpGet]
        [Route("consultarRecetas")]
        public HttpResponseMessage ConsultarRecetas()
        {
            List<Receta> recetas = ProcesadorReceta.ProcesoConsultarRecetas(1);
            if (recetas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, recetas);
            return response;
        }

        [HttpPost]
        [Route("agregarReceta")]
        public HttpResponseMessage AgregarReceta(Receta receta)
        {
            if (receta == null || ProcesadorReceta.ProcesoAgregarReceta(receta, 1))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpGet]
        [Route("consultarReceta/{nombre}")]
        public HttpResponseMessage ConsultarReceta(string nombre)
        {
            List<Receta> recetas = ProcesadorReceta.ProcesoConsultarReceta(nombre, 1);
            if (recetas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, recetas);
            return response;
        }

        [HttpPost]
        [Route("borrarReceta")]
        public HttpResponseMessage BorrarReceta(ObjGeneral nombre)
        {
            if (!ProcesadorReceta.ProcesoBorrarReceta(nombre.opcion, int.Parse(User.Identity.Name)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

    }
}
