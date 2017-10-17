using GasStationPharmacy.Models;
using System.Collections.Generic;
using GasStationPharmacy.Repositories;

namespace GasStationPharmacy.Processors
{
    /// <summary>
    /// Clase para procesar las peticiones http valida algunas restricciones
    /// realiza peticiones basadas en empleados
    /// </summary>
    public class ProcesadorEmpleado
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza la insercion de un empleado
        /// </summary>
        /// <param name="empleado">Modelo con el empleado a agregar</param>
        /// <returns>true si se inserta / false en caso contrario</returns>
        public static bool ProcesoAgregarEmpleado(EmpleadoRecibo empleado)
        {
            if (RepositorioEmpleado.ConsultarBorrado(empleado.cedula))
            {return RepositorioEmpleado.InsertarEmpleadoBorrado(empleado);}
            else {return RepositorioEmpleado.AgregarEmpleado(empleado);}
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los empleados de una compañia 
        /// </summary>
        /// <param name="compania">compañia a consultar los empleados</param>
        /// <returns>Lista con la informacion de todos los empleados de la compañia</returns>
        public static List<EmpleadoRecibo> ProcesoConsultarEmpleados(string compania)
        { return RepositorioEmpleado.ConsultarEmpleados(compania); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Verifica si un empleado existe y tiene rol Administrador o Farmaceutico
        /// </summary>
        /// <param name="cedula">cedula del empleado ingresada</param>
        /// <param name="contrasena">contrasena del empleado ingresada</param>
        /// <returns>true si el empleado existe 
        /// , false si no existe</returns>
        public static List<string> ProcesoLogearEmpleado(int cedula, string contrasena)
        { return RepositorioEmpleado.LogearEmpleado(cedula, contrasena); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado a borrar</param>
        /// <returns>true si lo borra / false en caso contrario</returns>
        public static bool ProcesoBorrarEmpleado(int cedula)
        {
            if (RepositorioEmpleado.ConsultarAdmin(cedula))
                return RepositorioEmpleado.BorrarEmpleado(cedula);
            return false;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Intenta actualizar la informacion de un empleado
        /// </summary>
        /// <param name="empleado">empleado a actualizar</param>
        /// <returns>true si se actualiza / false en caso contrario</returns>
        public static bool ProcesoActualizarEmpleado(EmpleadoRecibo empleado)
        { return RepositorioEmpleado.ActualizarEmpleado(empleado); }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta la compañia de un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado</param>
        /// <returns>string con la compañia del empleado</returns>
        public static ObjGeneral ProcesoConsultarCompañia(int cedula)
        { return RepositorioEmpleado.consultarCompañia(cedula); }
    }
}