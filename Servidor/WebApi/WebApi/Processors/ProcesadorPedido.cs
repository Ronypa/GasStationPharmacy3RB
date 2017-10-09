using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla clientes
    /// </summary>
    public class ProcesadorPedido
    {
        /// <summary>
        /// Consulta todos los pedidos de un usuario
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public static List<Pedido> ProcesoConsultarPedidos(int cedula)
        {return RepositorioPedido.ConsultarPedidos(cedula);}

        public static List<Pedido> ProcesoConsultarPedidosNuevos()
        { return RepositorioPedido.ConsultarPedidosNuevos(); }

        public static List<Pedido> ProcesoConsultarPedidosPreparados()
        { return RepositorioPedido.ConsultarPedidosPreparados(); }

        public static List<Pedido> ProcesoConsultarPedidosFacturados()
        { return RepositorioPedido.ConsultarPedidosFacturados(); }

        public static List<Pedido> ProcesoConsultarPedidosRetirados()
        { return RepositorioPedido.ConsultarPedidosRetirados(); }

        public static bool ProcesoPedidoPreparado(int numero)
        { return RepositorioPedido.PedidoPreparado(numero); }

        public static bool ProcesoPedidoFacturado(int numero)
        { return RepositorioPedido.PedidoFacturado(numero); }

        public static bool ProcesoPedidoRetirado(int numero)
        { return RepositorioPedido.PedidoRetirado(numero); }

        public static bool ProcesoBorrarPedido(string nombre, int cedula)
        { return RepositorioPedido.BorrarPedido(nombre,cedula); }

    }
}