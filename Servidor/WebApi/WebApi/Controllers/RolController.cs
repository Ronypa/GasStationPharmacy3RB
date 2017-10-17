using GasStationPharmacy.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Processors;

namespace GasStationPharmacy.Controllers
{
    /// <summary>
    /// Controlador que acepta peticiones http sobre los roles
    /// </summary>
    public class RolController : ApiController
    {

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los roles
        /// </summary>
        /// <returns>HTTP OK y la lista con la informacion de los roles</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("consultarRoles")]
        public HttpResponseMessage ConsultarRol()
        {
            List<Rol> roles = ProcesadorRol.ProcesoConsultarRol();
            if (roles == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, roles);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un rol
        /// </summary>
        /// <param name="obj">nombre del rol a borrar</param>
        /// <returns>HTTP OK si lo borra</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("borrarRol")]
        public HttpResponseMessage BorrarRol(ObjGeneral obj)
        {
            if (!ProcesadorRol.ProcesoBorrarRol(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega un rol nuevo
        /// </summary>
        /// <param name="rol">informacion del rol a agregar</param>
        /// <returns>HTTP OK si crea el rol</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("agregarRol")]
        public HttpResponseMessage AgregarRol(Rol rol)
        {
            if (rol == null || !ProcesadorRol.ProcesoAgregarRol(rol))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza un rol
        /// </summary>
        /// <param name="rol">informacion del rol a actualizar</param>
        /// <returns>HTTP OK si actualiza el rol</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("actualizarRol")]
        public HttpResponseMessage ActualizarRol(Rol rol)
        {
            if (!ProcesadorRol.ProcesoActualizarRol(rol))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

    }
}