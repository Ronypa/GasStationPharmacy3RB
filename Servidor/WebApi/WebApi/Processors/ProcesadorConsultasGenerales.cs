using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones
    /// Realiza algunas peticiones generales
    /// </summary>
    public class ProcesadorConsultasGenerales
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las compañias del sistema
        /// </summary>
        /// <returns>Lista con el nombre de las compañias</returns>
        public static List<ObjGeneral> ProcesoConsultarCompanias()
        { return RepositorioGeneral.ConsultarCompanias();}

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las sucursales de una compañia
        /// </summary>
        /// <param name="compania">compañia de la que se consultan las sucursales</param>
        /// <returns></returns>
        public static List<ObjGeneral> ProcesoConsultarSucursales(string compania)
        { return RepositorioGeneral.ConsultarSucursales(compania); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los roles del sistema
        /// </summary>
        /// <returns>Lista con los nombres de los roles</returns>
        public static List<ObjGeneral> ProcesoConsultarRoles()
        { return RepositorioGeneral.ConsultarRoles(); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los telefonos de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a consultar los telefonos</param>
        /// <returns>Lista con los telefonos del cliente</returns>
        public static List<ObjGeneral> ProcesoConsultarTelefonos(int cedula)
        { return RepositorioGeneral.ConsultarTelefonos(cedula); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las casas farmaceuticas del sistema
        /// </summary>
        /// <returns>Lista con los nombres de las casas farmaceuticas</returns>
        public static List<ObjGeneral> ProcesoConsultarCasasFarmaceuticas()
        { return RepositorioGeneral.ConsultarCasasFarmaceuticas(); }
    }
}