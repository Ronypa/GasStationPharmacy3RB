using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;
using System.Drawing;
using System.IO;

namespace WebApi.Controllers
{
    public class PedidosController : ApiController
    {

        [HttpGet]
        [Route("consultarPedidos")]
        public HttpResponseMessage ConsultarPedidos()
        {
            List<Pedido> pedidos = ProcesadorPedido.ProcesoConsultarPedidos(int.Parse(User.Identity.Name));
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        [HttpGet]
        [Route("consultarPedidosNuevos")]
        public HttpResponseMessage ConsultarPedidosNuevos()
        {
            List<Pedido> pedidos = ProcesadorPedido.ProcesoConsultarPedidosNuevos();
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        [HttpGet]
        [Route("consultarPedidosPreparados")]
        public HttpResponseMessage ConsultarPedidosPreparados()
        {
            List<Pedido> pedidos = ProcesadorPedido.ProcesoConsultarPedidosPreparados();
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        [HttpGet]
        [Route("consultarPedidosFacturados")]
        public HttpResponseMessage ConsultarPedidosFacturados()
        {
            List<Pedido> pedidos = ProcesadorPedido.ProcesoConsultarPedidosFacturados();
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        [HttpGet]
        [Route("consultarPedidosRetirados")]
        public HttpResponseMessage ConsultarPedidosRetirados()
        {
            List<Pedido> pedidos = ProcesadorPedido.ProcesoConsultarPedidosRetirados();
            if (pedidos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pedidos);
            return response;
        }

        [HttpPost]
        [Route("pedidoPreparado")]
        public HttpResponseMessage pedidoPreparado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoPreparado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("pedidoFacturado")]
        public HttpResponseMessage pedidoFacturado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoFacturado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("pedidoRetirado")]
        public HttpResponseMessage pedidoRetirado([FromBody]string numero)
        {
            if (!ProcesadorPedido.ProcesoPedidoRetirado(int.Parse(numero)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPost]
        [Route("borrarPedido")]
        public HttpResponseMessage BorrarPedido(ObjGeneral nombre)
        {
            if (!ProcesadorPedido.ProcesoBorrarPedido(nombre.opcion, int.Parse(User.Identity.Name)))
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