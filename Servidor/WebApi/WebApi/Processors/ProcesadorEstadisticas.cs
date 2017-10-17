using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones 
    /// consultas relacionadas con la seccion de estadisticas
    /// </summary>
    public class ProcesadorEstadisticas
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los 5 productos mas vendidos por la compañia
        /// </summary>
        /// <param name="compania">compañia a consultar</param>
        /// <returns>Lista con la cantidad y el producto de los 5 mas vendidos</returns>
        public static List<ObjGeneralEstadistica> ProcesoProductosCompania(string compania)
        { return RepositorioEstadistica.ConsultarProductosCompania(compania); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los 5 productos mas vendidos en general por el sistema
        /// </summary>
        /// <returns>La cantidad y nombre de los 5 productos mas vendidos</returns>
        public static List<ObjGeneralEstadistica> ProcesoProductosTotales()
        { return RepositorioEstadistica.ConsultarProductosTotal(); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta la cantidad de ventas de la compañia
        /// </summary>
        /// <param name="compania">compañia que consulta la informacion</param>
        /// <returns>cantidad de ventas de la compañia</returns>
        public static int ProcesoVentasTotales(string compania)
        { return RepositorioEstadistica.VentasTotales(compania); }
    }
}