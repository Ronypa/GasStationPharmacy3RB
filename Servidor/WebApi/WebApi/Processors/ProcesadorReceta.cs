using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla clientes
    /// </summary>
    public class ProcesadorReceta
    {
        /// <summary>
        /// Consulta todos los pedidos de un usuario
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public static List<Receta> ProcesoConsultarRecetas(int cedula)
        {return RepositorioReceta.ConsultarRecetas(cedula);}

        public static bool ProcesoAgregarReceta(Receta receta, int cliente)
        { return RepositorioReceta.AgregarReceta(receta, cliente); }

        public static List<Receta> ProcesoConsultarReceta(string nombre, int cedula)
        { return RepositorioReceta.ConsultarReceta(nombre,cedula); }

        public static bool ProcesoBorrarReceta(string nombre, int cedula)
        { return RepositorioReceta.BorrarReceta(nombre, cedula); }

    }
}