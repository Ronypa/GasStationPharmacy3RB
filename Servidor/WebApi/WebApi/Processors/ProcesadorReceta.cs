using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones 
    /// relacionado a las recetas
    /// </summary>
    public class ProcesadorReceta
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todas las recetas de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente que consulta las recetas</param>
        /// <returns>Lista con el numero y nombre de las recetas</returns>
        public static List<ObjGeneral> ProcesoConsultarRecetas(int cedula)
        { return RepositorioReceta.ConsultarRecetas(cedula); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega una nueva receta
        /// </summary>
        /// <param name="receta">receta a a agregar</param>
        /// <param name="cliente">cliente que solicita agregar la receta</param>
        /// <returns>true si agrega la receta</returns>
        public static bool ProcesoAgregarReceta(RecetaRecibo receta, int cliente)
        {
            if (RepositorioReceta.ConsultarBorrado(receta.numero))
            { return RepositorioReceta.ActualizarReceta(receta, cliente); }
            return RepositorioReceta.AgregarReceta(receta, cliente);
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta la informacion en detalle de una receta
        /// </summary>
        /// <param name="numero">numero de receta a consultar</param>
        /// <param name="cedula">cliente que solicita la receta</param>
        /// <returns></returns>
        public static List<RecetaEnvio> ProcesoConsultarReceta(int numero, int cedula)
        { return RepositorioReceta.ConsultarReceta(numero, cedula); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra una receta 
        /// </summary>
        /// <param name="numero">numero de receta a borrar</param>
        /// <param name="cedula">cliente que borra la receta</param>
        /// <returns></returns>
        public static bool ProcesoBorrarReceta(int numero, int cedula)
        {
            if (RepositorioReceta.ConsultarParaBorrar(numero))
            {return RepositorioReceta.BorrarReceta(numero, cedula);}
            return false;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Intenta actualizar una receta
        /// </summary>
        /// <param name="cedula">cedula del cliente</param>
        /// <param name="receta">receta a actualizar</param>
        /// <returns>true si la actualiza</returns>
        public static bool ProcesoActualizarReceta(int cedula, RecetaRecibo receta)
        {
            if (RepositorioReceta.ConsultarUso(receta.numero))
            {return RepositorioReceta.ActualizarReceta(receta, cedula);}
            return false;
        }
    }
}