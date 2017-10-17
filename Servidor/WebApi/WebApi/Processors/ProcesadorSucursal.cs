using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones 
    /// relacionado a las sucursales
    /// </summary>
    public class ProcesadorSucursal
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las sucursales de una compañia
        /// </summary>
        /// <param name="compania">compañia a consultar las sucursales</param>
        /// <returns></returns>
        public static List<Sucursal> ProcesoConsultarSucursal(string compania)
        { return RepositorioSucursal.ConsultarSucursal(compania); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra una sucursal
        /// </summary>
        /// <param name="nombre">nombre de la sucursal a borrar</param>
        /// <returns></returns>
        public static bool ProcesoBorrarSucursal(string nombre)
        {   if (RepositorioSucursal.ConsultarParaBorrarPorPedido(nombre) &&
                RepositorioSucursal.ConsultarParaBorrarPorEmpleado(nombre)){ 
                return RepositorioSucursal.BorrarSucursal(nombre);
            }
            return false;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega una nueva sucursal
        /// </summary>
        /// <param name="sucursal">Informacion de la sucursal a agregar</param>
        /// <returns>true si la agrega</returns>
        public static bool ProcesoAgregarSucursal(Sucursal sucursal)
        {
            if (RepositorioSucursal.ConsultarBorrado(sucursal.nombre)) {
                return RepositorioSucursal.ReinsertarSucursal(sucursal);
            }
            return RepositorioSucursal.AgregarSucursal(sucursal);
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal a actualizar</param>
        /// <returns>true si la actualiza</returns>
        public static bool ProcesoActualizarSucursal(Sucursal sucursal)
        { return RepositorioSucursal.ActualizarSucursal(sucursal); }
    }
}