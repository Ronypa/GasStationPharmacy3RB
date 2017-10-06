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

        /// <summary>
        /// Consulta todos los clientes
        /// </summary>
        /// <returns>Una lista con todos los clientes y un http status de de ok si se 
        /// proceso la solicitud o unauthorized en caso contrario
        /// </returns>
        [Authorize(Roles ="Cliente")]
        [HttpGet]
        [Route("consultarClientes")]
        public HttpResponseMessage ConsultarClientes()
        {
            List <Cliente> clientes = ProcesadorCliente.ProcesoConsultarClientes();
            if (clientes == null) {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, clientes);
            return response;
        }

        /// <summary>
        /// Realiza el proceso de agregar un cliente
        /// </summary>
        /// <param name="cliente">Modelo del cliente a agregar</param>
        /// <returns></returns>
        [HttpPost]
        [Route("agregarCliente")]
        public string AgregarCliente(Cliente cliente){
            if (cliente == null) { return "false"; }
            return ProcesadorCliente.ProcesarCliente(cliente);
        }
    }
}