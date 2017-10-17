using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    /// <summary>
    /// Realiza las acciones HTTP de consultas generales
    /// </summary>
    public class ConsultasController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las compañias del sistema
        /// </summary>
        /// <returns>HTTP OK y las compañias</returns>
        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        [Route("consultarCompanias")]
        public HttpResponseMessage ConsultarCompanias()
        {
            List<ObjGeneral> companias = ProcesadorConsultasGenerales.ProcesoConsultarCompanias();
            if (companias == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, companias);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las casa farmaceuticas
        /// </summary>
        /// <returns>HTTP ok y las casas farmaceuticas</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("consultarCasaFarmaceuticaC")]
        public HttpResponseMessage ConsultarCasasFarmaceuticas()
        {
            List<ObjGeneral> companias = ProcesadorConsultasGenerales.ProcesoConsultarCasasFarmaceuticas();
            if (companias == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, companias);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las sucursales de una compañia
        /// </summary>
        /// <param name="obj">compañia a consultar</param>
        /// <returns>HTTP OK y las sucursales</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("consultarSucursalesC")]
        public HttpResponseMessage ConsultarSucursalesCliente(ObjGeneral obj)
        {
            List<ObjGeneral> sucursales = ProcesadorConsultasGenerales.ProcesoConsultarSucursales(obj.opcion);
            if (sucursales == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sucursales);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los telefonos de un cliente
        /// </summary>
        /// <returns>HTTP OK y la lista de telefonos</returns>
        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        [Route("consultarTelefonosC")]
        public HttpResponseMessage ConsultarTelefonos()
        {
            List<ObjGeneral> telefonos = ProcesadorConsultasGenerales.ProcesoConsultarTelefonos(int.Parse(User.Identity.Name));
            if (telefonos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, telefonos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los roles del sistema
        /// </summary>
        /// <returns>HTTP OK y lista de roles</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("consultarRolesC")]
        public HttpResponseMessage ConsultarRoles()
        {
            List<ObjGeneral> roles = ProcesadorConsultasGenerales.ProcesoConsultarRoles();
            if (roles == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, roles);
            return response;
        }
    }
}