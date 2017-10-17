using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    public class RecetasController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta el nombre y numero de todas las recetas
        /// </summary>
        /// <returns>HTTP OK y la informacion de las recetas</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        [Route("consultarRecetas")]
        public HttpResponseMessage ConsultarRecetas()
        {
            List<ObjGeneral> recetas = ProcesadorReceta.ProcesoConsultarRecetas(int.Parse(User.Identity.Name));
            if (recetas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, recetas);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega un a nueva receta
        /// </summary>
        /// <param name="receta">receta a agregar</param>
        /// <returns>HTTP OK si la crea</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("agregarReceta")]
        public HttpResponseMessage AgregarReceta(RecetaRecibo receta)
        {
            if (receta == null || !ProcesadorReceta.ProcesoAgregarReceta(receta, int.Parse(User.Identity.Name)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta la informacion de una receta
        /// </summary>
        /// <param name="receta">numero de receta a consultar</param>
        /// <returns>HTTP OK y la informacion de la receta</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("consultarReceta")]
        public HttpResponseMessage ConsultarReceta(ObjGeneral receta)
        {
            List<RecetaEnvio> recetas = ProcesadorReceta.ProcesoConsultarReceta(int.Parse(receta.opcion), int.Parse(User.Identity.Name));
            if (recetas == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, recetas);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra una receta
        /// </summary>
        /// <param name="nombre">nombre de la receta a borrar</param>
        /// <returns>HTTP OK si la borra</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("borrarReceta")]
        public HttpResponseMessage BorrarReceta(ObjGeneral nombre)
        {
            if (!ProcesadorReceta.ProcesoBorrarReceta(int.Parse(nombre.opcion), int.Parse(User.Identity.Name)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza una receta
        /// </summary>
        /// <param name="receta">Informacion de la receta a actualizar</param>
        /// <returns>HTTP OK si lo actualiza</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("actualizarReceta")]
        public HttpResponseMessage ActualizarReceta(RecetaRecibo receta)
        {
            if (!ProcesadorReceta.ProcesoActualizarReceta(int.Parse(User.Identity.Name), receta))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, 1);
            return response;
        }
    }
}