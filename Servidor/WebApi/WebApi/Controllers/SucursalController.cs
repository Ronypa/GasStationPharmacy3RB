using GasStationPharmacy.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Processors;

namespace GasStationPharmacy.Controllers
{
    /// <summary>
    /// Controlador que acepta peticiones http sobre las sucursales
    /// </summary>
    public class SucursalController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consultar consulta todas las sucursales de una compañia
        /// </summary>
        /// <param name="compania">compañia a consultar</param>
        /// <returns>HTTP OK con las sucursales</returns>
        [Authorize(Roles = "Administrador,Cliente")]
        [HttpPost]
        [Route("consultarSucursales")]
        public HttpResponseMessage ConsultarSucursal(ObjGeneral compania)
        {
            List<Sucursal> sucursales = ProcesadorSucursal.ProcesoConsultarSucursal(compania.opcion);
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
        /// Borra una sucursal
        /// </summary>
        /// <param name="obj">nombre de la sucursal a borrar</param>
        /// <returns>HTTP OK si lo borra</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("borrarSucursal")]
        public HttpResponseMessage BorrarSucursal(ObjGeneral obj)
        {
            if (!ProcesadorSucursal.ProcesoBorrarSucursal(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega una sucursal nueva
        /// </summary>
        /// <param name="sucursal">sucursal a agregar</param>
        /// <returns>HTTP ok si la agrega</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("agregarSucursal")]
        public HttpResponseMessage AgregarSucursal(Sucursal sucursal)
        {
            if (sucursal == null || !ProcesadorSucursal.ProcesoAgregarSucursal(sucursal))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de una sucursal
        /// </summary>
        /// <param name="sucursal">informacion de la sucursal a actualizar</param>
        /// <returns>HTTP OK si lo actualiza</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("actualizarSucursal")]
        public HttpResponseMessage ActualizarSucursal(Sucursal sucursal)
        {
            if (!ProcesadorSucursal.ProcesoActualizarSucursal(sucursal))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}