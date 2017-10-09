using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones y demas de la tabla empleados
    /// </summary>
    public class ProcesadorEmpleado
    {
        /// <summary>
        /// Consulta todos los empleados
        /// </summary>
        /// <returns>Lista de todos los empleados</returns>
        public static List<Empleado> ProcesoConsultarEmpleados()
        { return RepositorioEmpleado.ConsultarEmpleados(); }

        /// <summary>
        /// Verifica si un empleado existe
        /// </summary>
        /// <param name="cedula">cedula del empleado ingresada</param>
        /// <param name="contrasena">contrasena del empleado ingresada</param>
        /// <returns>true si el empleado existe 
        /// , false si no existe</returns>
        public static List<string> ProcesoLogearEmpleado(int cedula, string contrasena)
        { return RepositorioEmpleado.LogearEmpleado(cedula, contrasena); }

        public static bool ProcesoBorrarEmpleado(int cedula)
        { return RepositorioEmpleado.BorrarEmpleado(cedula); }

    }
}