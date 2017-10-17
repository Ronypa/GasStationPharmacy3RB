using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones
    /// relacionado a los medicamentos
    /// </summary>
    public class ProcesadorMedicamento
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los pedidos de un usuario
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public static List<Medicamento> ProcesoConsultarMedicamentos()
        {return RepositorioMedicamento.ConsultarMedicamento();}

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Intenta borrar un medicamento
        /// </summary>
        /// <param name="nombre">nombre del medicamento a borrar</param>
        /// <returns>true si lo borra / false en caso contrario</returns>
        public static bool ProcesoBorrarMedicamento(string nombre)
        {
            if (RepositorioMedicamento.ConsultarParaBorrarPorPedido(nombre))
            {return RepositorioMedicamento.BorrarMedicamento(nombre);}
            return false;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Intenta agregar un medicamento
        /// </summary>
        /// <param name="medicamento">informacion del medicamento a agregar</param>
        /// <returns>true si lo borra / false en caso contrario</returns>
        public static bool ProcesoAgregarMedicamento(Medicamento medicamento)
        {
            if (RepositorioMedicamento.ConsultarBorrado(medicamento.nombre))
            {return RepositorioMedicamento.ActualizarMedicamento(medicamento);}
            return RepositorioMedicamento.AgregarMedicamento(medicamento);
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de un medicamento
        /// </summary>
        /// <param name="medicamento">medicamento a actualizar</param>
        /// <returns>true si lo actualiza / false en caso contrario</returns>
        public static bool ProcesoActualizarMedicamento(Medicamento medicamento)
        { return RepositorioMedicamento.ActualizarMedicamento(medicamento); }
    }
}