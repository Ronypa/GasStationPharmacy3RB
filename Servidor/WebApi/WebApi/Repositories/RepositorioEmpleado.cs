using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de empleados 
    /// </summary>
    public class RepositorioEmpleado
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        /// <summary>
        /// Verifica si existe un empleado en base de datos
        /// </summary>
        /// <param name="cedula">cedula del empleado ingresada</param>
        /// <param name="contrasena">contraseña del empleado ingresada</param>
        /// <returns>true si el empleado existe
        /// false en caso contrario</returns>
        public static List<string> LogearEmpleado(int cedula, string contrasena)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1], [Apellido1] FROM[dbo].[EMPLEADO] " +
                "WHERE Cedula= @cedula AND Contraseña= @contrasena";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@contrasena", contrasena);
                var reader = comando.ExecuteReader();
                //No se encuentra el empleado
                if (!reader.HasRows) { conexion.Close(); return null; }
                //Si se encontro el empleado
                conexion.Close();
                return consultarRoles(cedula);
            }
            catch (Exception) { return null; }
        }

        public static List<string> consultarRoles(int cedula)
        {
            var lista = new List<string>();
            //query de la solicitud
            var query = "SELECT [Rol] FROM[dbo].[ROLXDEPENDIENTE] " +
                "WHERE Dependiente= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(reader.GetString(0));
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }


        //Consulta todos los empleado de la base de datos
        public static List<Empleado> ConsultarEmpleados()
        {
            //Lista que almacena los empleado
            var lista = new List<Empleado>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Señas], [FechaNacimiento], [Sucursal] FROM[dbo].[EMPLEADO] WHERE Activo=1";

            //Se ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Empleado
                        {
                            cedula = reader.GetInt32(0),
                            nombre1 = reader.GetString(1),
                            nombre2 = reader.GetString(2),
                            apellido1 = reader.GetString(3),
                            apellido2 = reader.GetString(4),
                            provincia = reader.GetString(5),
                            ciudad = reader.GetString(6),
                            señas = reader.GetString(7),
                            fechaNacimiento = reader.GetDateTime(8).ToString(),
                            sucursal = reader.GetString(9).ToString(),
                            contrasena = "",
                            activo = true
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static bool BorrarEmpleado(int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[EMPLEADO] SET [ACTIVO]=0" +
                "WHERE Cedula= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }


    }
}