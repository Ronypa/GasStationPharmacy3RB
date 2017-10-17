using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para gestion de roles
    /// </summary>
    public class RepositorioRol
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS;" +
            " Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);
        
        /// <summary>
        /// Consulta todos los roles
        /// </summary>
        /// <returns>Lista con la informacion de los roles</returns>
        public static List<Rol> ConsultarRoles()
        {
            //Lista que almacena los roles
            var lista = new List<Rol>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Descripcion] " +
                "FROM[dbo].[ROL] WHERE Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Rol
                        {
                            nombre = GetValue<string>(reader["nombre"]),
                            descripcion = GetValue<string>(reader["descripcion"])
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Elimina un rol de la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del rol a borrar</param>
        /// <returns>True si lo borra / false en caso contrario</returns>
        public static bool BorrarRol(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[ROL] SET [ACTIVO]=0" +
                "WHERE Nombre= @nombre";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Agrega un nuevo rol a la base de datos
        /// </summary>
        /// <param name="rol">Rol a agregar</param>
        /// <returns>True si lo pruedo agregar / false en caso contrario</returns>
        public static bool AgregarRol(Rol rol)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[ROL] ([Nombre] ,[Descripcion], [Activo]) " +
                "VALUES(@Nombre, @Descripcion, @Activo)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Nombre", rol.nombre);
                comando.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Actualiza la descripcion de un rol existente
        /// </summary>
        /// <param name="rol">rol a modificar</param>
        /// <returns>true si lo modifica / false en caso contrario</returns>
        public static bool ActualizarRol(Rol rol)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[ROL] SET [Descripcion] = @Descripcion, [Activo]=1 " +
                "WHERE Nombre=@Nombre";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                comando.Parameters.AddWithValue("@Nombre", rol.nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un rol esta borrado (para cuando se quiere crear un rol)
        /// </summary>
        /// <param name="nombre">nombre del rol a buscar</param>
        /// <returns>true si el rol existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Descripcion] FROM[dbo].[ROL] " +
                "WHERE Activo=0 AND Nombre= @nombre";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                var reader = comando.ExecuteReader();
                //No se encuentra el rol
                if (!reader.HasRows) { conexion.Close(); return false; }
                //Si se encontro el rol
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un rol se puede borrar (si ningun empleado usa el rol)
        /// </summary>
        /// <param name="nombre">nombre del rol a buscar</param>
        /// <returns>true si el rol se puede borrar
        /// false en caso contrario</returns>
        public static bool ConsultarParaBorrar(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1] FROM[dbo].[EMPLEADO] " +
                "WHERE Activo=1 AND Rol= @nombre";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                var reader = comando.ExecuteReader();
                //No se encuentra el rol
                if (!reader.HasRows) { conexion.Close(); return true;  }
                //Si se encontro el rol
                conexion.Close();
                return false;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Obtiene los valores de una consulta de la BD y verifica nulos
        /// </summary>
        /// <typeparam name="T">Generico para obtener varios tipos</typeparam>
        /// <param name="value">valor obtenido</param>
        /// <returns>el valor leido o null</returns>
        private static T GetValue<T>(object value)
        {
            return value == DBNull.Value
                   ? default(T)
                   : (T)Convert.ChangeType(value, typeof(T));
        }

    }
}