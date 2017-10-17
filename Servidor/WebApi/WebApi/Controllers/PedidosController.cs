using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    /// <summary>
    /// Recibe solicitudes relacionados con los pedidos
    /// </summary>
    public class PedidosController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los pedidos de un cliente
        /// </summary>
        /// <returns>HTTP OK y los pedidos del cliente</returns>
        [Authorize(Roles = "Cliente")]
        [HttpGet]
        [Route("consultarPedidos")]
        public HttpResponseMessage ConsultarPedidos()
        {
            List<ObjGeneral2> pedidos = ProcesadorPedido.ProcesoConsultarPedidos(int.Parse(User.Identity.Name));
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos nuevos de un sucursal 
        /// </summary>
        /// <param name="obj">nombre de la sucursal</param>
        /// <returns>Http OK con los pedidos nuevos de la sucursal</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("consultarPedidosNuevos")]
        public HttpResponseMessage ConsultarPedidosNuevos(ObjGeneral obj)
        {
            List<PedidoEnvio> pedidos = ProcesadorPedido.ProcesoConsultarPedidosNuevos(obj.opcion);
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos preparados de un sucursal 
        /// </summary>
        /// <param name="obj">nombre de la sucursal</param>
        /// <returns>Http OK con los pedidos preparados de la sucursal</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("consultarPedidosPreparados")]
        public HttpResponseMessage ConsultarPedidosPreparados(ObjGeneral obj)
        {
            List<PedidoEnvio> pedidos = ProcesadorPedido.ProcesoConsultarPedidosPreparados(obj.opcion);
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos facturados de un sucursal 
        /// </summary>
        /// <param name="obj">nombre de la sucursal</param>
        /// <returns>Http OK con los pedidos facturados de la sucursal</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("consultarPedidosFacturados")]
        public HttpResponseMessage ConsultarPedidosFacturados(ObjGeneral obj)
        {
            List<PedidoEnvio> pedidos = ProcesadorPedido.ProcesoConsultarPedidosFacturados(obj.opcion);
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos retirados de un sucursal 
        /// </summary>
        /// <param name="obj">nombre de la sucursal</param>
        /// <returns>Http OK con los pedidos retirados de la sucursal</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("consultarPedidosRetirados")]
        public HttpResponseMessage ConsultarPedidosRetirados(ObjGeneral obj)
        {
            List<PedidoEnvio> pedidos = ProcesadorPedido.ProcesoConsultarPedidosRetirados(obj.opcion);
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Modifica el estado del pedido lo pone como preparado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar</param>
        /// <returns>HTTP ok si le cambia el estado</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("pedidoPreparado")]
        public HttpResponseMessage pedidoPreparado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoPreparado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta el detalle de un pedido
        /// </summary>
        /// <param name="numero">numero del pedido a buscar</param>
        /// <returns>HTTP ok y la informacion del pedido</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("consultarDetallePedido")]
        public HttpResponseMessage ConsultarDetallePedido(ObjGeneral pedido)
        {
            List<PedidoEnvio> pedidos = ProcesadorPedido.ProcesoConsultarDetallePedido(int.Parse(pedido.opcion));
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Modifica el estado del pedido lo pone como facturado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar</param>
        /// <returns>HTTP ok si le cambia el estado</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("pedidoFacturado")]
        public HttpResponseMessage pedidoFacturado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoFacturado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Modifica el estado del pedido lo pone como retirado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar</param>
        /// <returns>HTTP ok si le cambia el estado</returns>
        [Authorize(Roles = "Farmaceutico")]
        [HttpPost]
        [Route("pedidoRetirado")]
        public HttpResponseMessage pedidoRetirado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoRetirado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un pedido
        /// </summary>
        /// <param name="nombre">nombre del pedido a borrar</param>
        /// <returns>HTTP ok si lo borra</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("borrarPedido")]
        public HttpResponseMessage BorrarPedido(ObjGeneral nombre)
        {
            if (!ProcesadorPedido.ProcesoBorrarPedido(int.Parse(nombre.opcion)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Crea un pedido nuevo 
        /// </summary>
        /// <param name="pedido">informacion el pedido a agregar</param>
        /// <returns>HTTP OK si lo agrega</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("agregarPedido")]
        public HttpResponseMessage AgregarPedido(PedidoRecibo pedido)
        {
            if (pedido == null || !ProcesadorPedido.ProcesoAgregarPedido(pedido, int.Parse(User.Identity.Name)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,1);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de un pedido
        /// </summary>
        /// <param name="pedido">Informacio del pedido a actualizar</param>
        /// <returns>HTTP OK se lo actualiza</returns>
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [Route("actualizarPedido")]
        public HttpResponseMessage ActualizarReceta(PedidoRecibo pedido)
        {
            if (!ProcesadorPedido.ProcesoActualizarPedido(pedido))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, 1);
            return response;
        }
    }
}