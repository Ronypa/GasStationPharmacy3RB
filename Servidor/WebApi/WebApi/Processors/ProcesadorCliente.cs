using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones
    /// para peticiones relacionadas a los clientes
    /// </summary>
    public class ProcesadorCliente
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza la insercion de un cliente
        /// </summary>
        /// <param name="cliente">Modelo con el cliente a agregar</param>
        /// <returns>true si lo agrega / false en caso contrario</returns>
        public static bool ProcesarCliente(ClienteRecibo cliente)
        {
            if (RepositorioCliente.ConsultarBorrado(cliente.cedula)) {
                if (RepositorioCliente.AgregarContraseña(cliente.cedula, cliente.contrasena))
                    return RepositorioCliente.ActualizarCliente(cliente);
            }
            return RepositorioCliente.AgregarCliente(cliente);
        }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta todos los clientes
        /// </summary>
        /// <returns>Lista de todos los clientes</returns>
        public static List<ClienteRecibo> ProcesoConsultarClientes()
        {return RepositorioCliente.ConsultarClientes();}

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Verifica si un cliente existe
        /// </summary>
        /// <param name="cedula">cedula del cliente ingresada</param>
        /// <param name="contrasena">contrasena del cliente ingresada</param>
        /// <returns>true si el cliente existe / false si no existe</returns>
        public static bool ProcesoLogearCliente(int cedula, string contrasena)
        {return RepositorioCliente.LogearCliente(cedula, contrasena);}

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Intenta borrar un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar</param>
        /// <returns>true si se borrar / false en caso contrario</returns>
        public static bool ProcesoBorrarCliente(int cedula)
        { return RepositorioCliente.BorrarCliente(cedula); }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Intenta actualizar la contraseña de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a modiificar la contraseña</param>
        /// <param name="vieja">contraseña actual del cliente</param>
        /// <param name="nueva">>contraseña nueva del cliente</param>
        /// <returns>true si se actualiza / false en caso contrario</returns>
        public static bool ProcesoCambiarContra(int cedula,string vieja, string nueva)
        { return RepositorioCliente.ActualizarContraseña(cedula, vieja, nueva); }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta toda la informacion de un cliente 
        /// </summary>
        /// <param name="cedula">cedula del cliente a consultar</param>
        /// <returns>Lista con la informacion del cliente</returns>
        public static List<ClienteEnvio> ProcesoConsultarModCliente(int cedula)
        { return RepositorioCliente.ConsultarModClientes(cedula); }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Actualiza la informacion de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a actualzar</param>
        /// <param name="cliente">informacion a actualizar del cliente</param>
        /// <returns>true si se actualiza / false en caso contrario</returns>
        public static bool ProcesoActualizarCliente(int cedula, ClienteRecibo cliente)
        { cliente.cedula = cedula;  return RepositorioCliente.ActualizarCliente(cliente); }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Actualiza la informacion del cliente desde la pagina del administrador
        /// </summary>
        /// <param name="cliente">informacion del cliente a modificar</param>
        /// <returns></returns>
        public static bool ProcesoActualizarClienteAdmin(ClienteRecibo cliente)
        { return RepositorioCliente.ActualizarClienteAdmin(cliente); }
    }
}