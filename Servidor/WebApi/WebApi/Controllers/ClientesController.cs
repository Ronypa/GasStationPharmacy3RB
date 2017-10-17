using GasStationPharmacy.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Processors;

namespace GasStationPharmacy.Controllers
{
    /// <summary>
    /// Controlador que acepta peticiones http sobre la tabla de clientes
    /// </summary>
    public class ClientesController : ApiController
    {

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los clientes
        /// </summary>
        /// <returns>Una lista con todos los clientes y un http status de de ok si se 
        /// proceso la solicitud o unauthorized en caso contrario
        /// </returns>
        [Authorize (Roles ="Administrador")]
        [HttpGet]
        [Route("consultarClientes")]
        public HttpResponseMessage ConsultarClientes()
        {
            List<ClienteRecibo> clientes = ProcesadorCliente.ProcesoConsultarClientes();
            if (clientes == null) {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, clientes);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza el proceso de agregar un cliente
        /// </summary>
        /// <param name="cliente">Modelo del cliente a agregar</param>
        /// <returns></returns>
        [HttpPost]
        [Route("agregarCliente")]
        public HttpResponseMessage AgregarCliente(ClienteRecibo cliente) {
            if (cliente == null || !ProcesadorCliente.ProcesarCliente(cliente)) {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Solicita borrar un cliente desde el administrador
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar</param>
        /// <returns>HTTP OK si lo borra</returns>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [Route("borrarClientes")]
        public HttpResponseMessage BorrarCliente([FromBody]string cedula)
        {
            if (!ProcesadorCliente.ProcesoBorrarCliente(int.Parse(cedula)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Solicita borrar un cliente desde el cliente
        /// </summary>
        /// <param name="obj">cedula del cliente a borrar</param>
        /// <returns>HTTP OK si lo borra</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("borrarCliente")]
        public HttpResponseMessage AutoBorrarCliente(ObjGeneral obj)
        {
            if (!ProcesadorCliente.ProcesoBorrarCliente(int.Parse(User.Identity.Name)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("actualizarContrasena")]
        public HttpResponseMessage ActualizarContra(ObjGeneral obj)
        {
            if (!ProcesadorCliente.ProcesoCambiarContra(int.Parse(User.Identity.Name),obj.opcion,obj.opcion2))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// consulta la informacion del cliente 
        /// </summary>
        /// <returns>HTTP OK si lo consulta y la informacion del cliente</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet]
        [Route("consultarCliente")]
        public HttpResponseMessage ConsultarCliente()
        {
            List<ClienteEnvio> clientes = ProcesadorCliente.ProcesoConsultarModCliente(int.Parse(User.Identity.Name));
            if (clientes == null) {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, clientes);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza los datos del cliente
        /// </summary>
        /// <param name="cliente">informacion del cliente a actualizar</param>
        /// <returns>HTTP OK si lo actualiza</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("actualizarDatos")]
        public HttpResponseMessage ActualizarCliente(ClienteRecibo cliente)
        {
            if (!ProcesadorCliente.ProcesoActualizarCliente(int.Parse(User.Identity.Name), cliente))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }
        
    }
}