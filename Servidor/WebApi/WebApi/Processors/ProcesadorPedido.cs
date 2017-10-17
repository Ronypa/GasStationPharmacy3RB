using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones 
    /// relacionado a los pedidos
    /// </summary>
    public class ProcesadorPedido
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los pedidos de un cliente
        /// </summary>
        /// <param name="cedula">cliente que consulta los pedidos</param>
        /// <returns>lista con el nombre y numero de los pedidos</returns>
        public static List<ObjGeneral2> ProcesoConsultarPedidos(int cedula)
        {return RepositorioPedido.ConsultarPedidos(cedula);}

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos nuevos de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal que consulta los medidos</param>
        /// <returns>Lista con la informacion de los pedidos</returns>
        public static List<PedidoEnvio> ProcesoConsultarPedidosNuevos(string sucursal)
        { return RepositorioPedido.ConsultarPedidosPorEstado('n', sucursal); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos preparados de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal que consulta los medidos</param>
        /// <returns>Lista con la informacion de los pedidos</returns>
        public static List<PedidoEnvio> ProcesoConsultarPedidosPreparados(string sucursal)
        { return RepositorioPedido.ConsultarPedidosPorEstado('p', sucursal); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos facturados de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal que consulta los medidos</param>
        /// <returns>Lista con la informacion de los pedidos</returns>
        public static List<PedidoEnvio> ProcesoConsultarPedidosFacturados(string sucursal)
        { return RepositorioPedido.ConsultarPedidosPorEstado('f', sucursal); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los pedidos retirados de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal que consulta los medidos</param>
        /// <returns>Lista con la informacion de los pedidos</returns>
        public static List<PedidoEnvio> ProcesoConsultarPedidosRetirados(string sucursal)
        { return RepositorioPedido.ConsultarPedidosPorEstado('r', sucursal); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Pone un pedido en el estado de preparado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar el estado</param>
        /// <returns>true si le cambia el estado / false en caso contrario</returns>
        public static bool ProcesoPedidoPreparado(int numero)
        { return RepositorioPedido.CambiarEstadoPedido(numero,'p'); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Pone un pedido en el estado de facturado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar el estado</param>
        /// <returns>true si le cambia el estado / false en caso contrario</returns>
        public static bool ProcesoPedidoFacturado(int numero)
        { return RepositorioPedido.CambiarEstadoPedido(numero,'f'); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Pone un pedido en el estado de retirado
        /// </summary>
        /// <param name="numero">numero del pedido a modificar el estado</param>
        /// <returns>true si le cambia el estado / false en caso contrario</returns>
        public static bool ProcesoPedidoRetirado(int numero)
        { return RepositorioPedido.CambiarEstadoPedido(numero, 'r'); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un pedido que ya esta retirado o esta como nuevo
        /// </summary>
        /// <param name="numero">numero del pedido a borrar</param>
        /// <returns>true si lo borra / false en caso contrario</returns>
        public static bool ProcesoBorrarPedido(int numero)
        {
            if (RepositorioPedido.ConsultarParaActualizar(numero, "r") || RepositorioPedido.ConsultarParaActualizar(numero, "n"))
            {
                return RepositorioPedido.BorrarPedido(numero);
            }
            return false;
        }

        /// <summary>
        /// Agrega un nuevo pedido
        /// </summary>
        /// <param name="pedido">pedido a agregar</param>
        /// <param name="cliente">cliente que agrega el pedido</param>
        /// <returns>true si lo agrega / false en caso contrario</returns>
        public static bool ProcesoAgregarPedido(PedidoRecibo pedido, int cliente)
        { pedido.cliente = cliente;  return RepositorioPedido.AgregarPedido(pedido); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// consulta informacion del pedido en detalle
        /// </summary>
        /// <param name="pedido">pedido a consultar</param>
        /// <returns>lista con la informacion del pedido consultado</returns>
        public static List<PedidoEnvio> ProcesoConsultarDetallePedido(int pedido)
        { return RepositorioPedido.ConsultarDetallePedido(pedido); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Intenta actualizar un pedido
        /// </summary>
        /// <param name="pedido">pedido a actualizar</param>
        /// <returns>true si lo actualiza</returns>
        public static bool ProcesoActualizarPedido(PedidoRecibo pedido)
        {
            if (RepositorioPedido.ConsultarParaActualizar(pedido.numero,"n"))
            {
                return RepositorioPedido.ActualizarPedido(pedido);
            }
            return false;
        }

    }
}