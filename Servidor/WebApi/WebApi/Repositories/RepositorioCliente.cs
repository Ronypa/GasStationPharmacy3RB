using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de clientes 
    /// </summary>
    public class RepositorioCliente
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        /// <summary>
        /// Verifica si existe un cliente en base de datos
        /// </summary>
        /// <param name="cedula">cedula del cliente ingresada</param>
        /// <param name="contrasena">contraseña del cliente ingresada</param>
        /// <returns>true si el cliente existe
        /// false en caso contrario</returns>
        public static bool LogearCliente(int cedula, string contrasena)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1], [Apellido1] FROM[dbo].[CLIENTE] " +
                "WHERE Cedula= @cedula AND Contraseña= @contrasena";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@contrasena", contrasena);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (!reader.HasRows) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }


        //Consulta todos los clientes de la base de datos
        public static List<Cliente> ConsultarClientes()
        {
            //Lista que almacena los clientes
            var lista = new List<Cliente>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Señas], [FechaNacimiento], [Prioridad] FROM[dbo].[CLIENTE] WHERE Activo=1";

            //Se ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Cliente {cedula = reader.GetInt32(0),
                            nombre1 = reader.GetString(1),
                            nombre2 = reader.GetString(2),
                            apellido1 = reader.GetString(3),
                            apellido2 = reader.GetString(4),
                            provincia = reader.GetString(5),
                            ciudad = reader.GetString(6),
                            señas = reader.GetString(7),
                            fechaNacimiento = reader.GetDateTime(8).ToString(),
                            contrasena = "",
                            activo = true
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        /// <summary>
        /// Crea un cliente nuevo
        /// </summary>
        /// <param name="cliente">Modelo de cliente a insertar</param>
        /// <returns></returns>
        public static string AgregarCliente(Cliente cliente)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[CLIENTE] ([Cedula] ,[Nombre1], [Nombre2] ,[Apellido1], " +
                "[Apellido2] ,[Provincia] ,[Cuidad] ,[Señas] ,[FechaNacimiento] ,[Prioridad] , " +
                "[Contraseña] ,[Activo]) VALUES(@Cedula, @Nombre1, @Nombre2, @Apellido1," +
                "@Apellido2, @Provincia, @Cuidad, @Señas, @FechaNacimiento, @Prioridad," +
                "@Contraseña, @Activo)";

            //Se ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", cliente.cedula);
                comando.Parameters.AddWithValue("@Contraseña", cliente.contrasena);
                comando.Parameters.AddWithValue("@Nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@Nombre2", cliente.nombre2);
                comando.Parameters.AddWithValue("@Apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@Apellido2", cliente.apellido2);
                comando.Parameters.AddWithValue("@Provincia", cliente.provincia);
                comando.Parameters.AddWithValue("@Cuidad", cliente.ciudad);
                comando.Parameters.AddWithValue("@Señas", cliente.señas);
                comando.Parameters.AddWithValue("@FechaNacimiento", cliente.fechaNacimiento);
                comando.Parameters.AddWithValue("@Prioridad", 1);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return "true";
            }
            catch (Exception e) { return e.ToString(); }
        }
    }
}