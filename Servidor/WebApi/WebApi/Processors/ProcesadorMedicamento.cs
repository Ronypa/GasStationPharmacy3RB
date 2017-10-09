using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla clientes
    /// </summary>
    public class ProcesadorMedicamento
    {
        /// <summary>
        /// Consulta todos los pedidos de un usuario
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public static List<Medicamento> ProcesoConsultarMedicamentos()
        {return RepositorioMedicamento.ConsultarMedicamento();}

        public static bool ProcesoBorrarMedicamento(string nombre)
        { return RepositorioMedicamento.BorrarMedicamento(nombre); }
    }
}