using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla empleados
    /// </summary>
    public class ProcesadorRol
    {

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los roles del sistema
        /// </summary>
        /// <returns>Lista con la informacion de todos los roles</returns>
        public static List<Rol> ProcesoConsultarRol()
        { return RepositorioRol.ConsultarRoles(); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un rol del sistema
        /// </summary>
        /// <param name="nombre">nombre del rol a borrar</param>
        /// <returns>true si lo borra</returns>
        public static bool ProcesoBorrarRol(string nombre)
        {
            if (RepositorioRol.ConsultarParaBorrar(nombre)) {
                return RepositorioRol.BorrarRol(nombre);
            }
            return false;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega un nuevo rol
        /// </summary>
        /// <param name="rol">informacion del rol a agregar</param>
        /// <returns>true si lo crea</returns>
        public static bool ProcesoAgregarRol(Rol rol)
        {
            if (RepositorioRol.ConsultarBorrado(rol.nombre)){
                return RepositorioRol.ActualizarRol(rol);
            }
            return RepositorioRol.AgregarRol(rol);
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la descripcion de un rol
        /// </summary>
        /// <param name="rol">Informacion del rol a modificar</param>
        /// <returns>true si lo actualiza</returns>
        public static bool ProcesoActualizarRol(Rol rol)
        { return RepositorioRol.ActualizarRol(rol); }
    }
}