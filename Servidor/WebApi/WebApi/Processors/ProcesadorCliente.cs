using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla clientes
    /// </summary>
    public class ProcesadorCliente
    {
        /// <summary>
        /// Realiza la insercion de un cliente
        /// </summary>
        /// <param name="cliente">Modelo con el cliente a agregar</param>
        /// <returns></returns>
        public static string ProcesarCliente(Cliente cliente)
        {return RepositorioCliente.AgregarCliente(cliente);}

        /// <summary>
        /// Consulta todos los clientes
        /// </summary>
        /// <returns>Lista de todos los clientes</returns>
        public static List<Cliente> ProcesoConsultarClientes()
        {return RepositorioCliente.ConsultarClientes();}

        /// <summary>
        /// Verifica si un cliente existe
        /// </summary>
        /// <param name="cedula">cedula del cliente ingresada</param>
        /// <param name="contrasena">contrasena del cliente ingresada</param>
        /// <returns>true si el cliente existe 
        /// , false si no existe</returns>
        public static bool ProcesoLogearCliente(int cedula, string contrasena)
        {return RepositorioCliente.LogearCliente(cedula, contrasena);}
    }
}