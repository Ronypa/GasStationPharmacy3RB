using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de sucursales 
    /// </summary>
    public class RepositorioSucursal
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);
        
        /// <summary>
        /// Consulta todas las sucursales de una compañia
        /// </summary>
        /// <returns>Lista con la informacion de las companias</returns>
        public static List<Sucursal> ConsultarSucursal(string compania)
        {
            //Lista que almacena los empleado
            var lista = new List<Sucursal>();
            //Query que consulta la base de datos
            var query = "SELECT [SUCURSAL].Nombre, [SUCURSAL].Provincia, [SUCURSAL].Cuidad, [SUCURSAL].Señas," +
                " [SUCURSAL].Descripcion, [ADMINISTRADORXSUCURSAL].Administrador AS admin " +
                "FROM[dbo].[SUCURSAL] INNER JOIN ADMINISTRADORXSUCURSAL " +
                "ON [SUCURSAL].Nombre=[ADMINISTRADORXSUCURSAL].Sucursal WHERE SUCURSAL.Activo=1 AND Compañia=@compania";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@compania", compania);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Sucursal
                        {
                            nombre = GetValue<string>(reader["nombre"]),
                            provincia = GetValue<string>(reader["provincia"]),
                            ciudad = GetValue<string>(reader["cuidad"]),
                            senas = GetValue<string>(reader["señas"]),
                            descripcion = GetValue<string>(reader["descripcion"]),
                            administrador = GetValue<int>(reader["admin"])
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borrado logico de un sucursal
        /// </summary>
        /// <param name="nombre">nombre de la sucursal a borrar</param>
        /// <returns></returns>
        public static bool BorrarSucursal(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[SUCURSAL] SET [ACTIVO]=0" +
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
                BorrarPedidos(nombre);
                BorrarAdmin(nombre);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega una sucursal nueva
        /// </summary>
        /// <param name="sucursal"></param>
        /// <returns></returns>
        public static bool AgregarSucursal(Sucursal sucursal)
        {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[SUCURSAL] ([Nombre] ,[Provincia], [Cuidad], " +
                    "[Señas], [Descripcion] ,[Compañia] ,[Activo]) VALUES(@Nombre, @Provincia," +
                    " @Cuidad, @Señas, @Descripcion, @Compañia, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Nombre", sucursal.nombre);
                    comando.Parameters.AddWithValue("@Provincia", sucursal.provincia);
                    if (string.IsNullOrEmpty(sucursal.ciudad))
                        comando.Parameters.AddWithValue("@Cuidad", DBNull.Value);
                    else
                        comando.Parameters.AddWithValue("@Cuidad", sucursal.ciudad);
                    if (string.IsNullOrEmpty(sucursal.senas))
                        comando.Parameters.AddWithValue("@Señas", DBNull.Value);
                    else
                        comando.Parameters.AddWithValue("@Señas", sucursal.senas);
                    if (string.IsNullOrEmpty(sucursal.descripcion))
                        comando.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                    else
                        comando.Parameters.AddWithValue("@Descripcion", sucursal.descripcion);
                    comando.Parameters.AddWithValue("@Compañia", sucursal.compania);
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                    AgregarAdministrador(sucursal);
                    return true;
                }
                catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega el administrador de la sucursal
        /// </summary>
        /// <param name="sucursal">sucursal a agregar el administrador</param>
        /// <returns></returns>
        public static bool AgregarAdministrador(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL] ([Administrador] ,[Sucursal], " +
                "[Activo]) VALUES(@Admin, @Sucursal,@Activo)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Admin", sucursal.administrador);
                comando.Parameters.AddWithValue("@Sucursal", sucursal.nombre);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza el administrador de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal a modificar</param>
        /// <returns>true si lo actualiza / false en caso contrario</returns>
        public static bool ActualizarAdministrador(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[ADMINISTRADORXSUCURSAL] SET [Administrador] = @Administrador " +
                "WHERE Sucursal=@Sucursal";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Sucursal", sucursal.nombre);
                comando.Parameters.AddWithValue("@Administrador", sucursal.administrador);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal a modificar</param>
        /// <returns>true si lo actualiza / false en caso contrario</returns>
        public static bool ActualizarSucursal(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[SUCURSAL] SET [Provincia] = @Provincia, [Cuidad]=@Ciudad, " +
                "[Señas]=@Senas, [Descripcion]=@Descripcion " +
                "WHERE Nombre=@Nombre AND Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Provincia", sucursal.provincia);
                if (string.IsNullOrEmpty(sucursal.ciudad))
                    comando.Parameters.AddWithValue("@Ciudad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Ciudad", sucursal.ciudad);
                if (string.IsNullOrEmpty(sucursal.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", sucursal.senas);
                if (string.IsNullOrEmpty(sucursal.descripcion))
                    comando.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Descripcion", sucursal.descripcion);

                comando.Parameters.AddWithValue("@Nombre", sucursal.nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                ActualizarAdministrador(sucursal);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de una sucursal
        /// </summary>
        /// <param name="sucursal">sucursal a reinsertar</param>
        /// <returns>true si la inserta / false en caso contrario</returns>
        public static bool ReinsertarSucursal(Sucursal sucursal)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[SUCURSAL] SET [Provincia] = @Provincia, [Cuidad]=@Ciudad, " +
                "[Señas]=@Senas, [Descripcion]=@Descripcion, [Compañia]=@Compania" +
                "WHERE Nombre=@Nombre AND Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Provincia", sucursal.provincia);
                if (string.IsNullOrEmpty(sucursal.ciudad))
                    comando.Parameters.AddWithValue("@Ciudad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Ciudad", sucursal.ciudad);
                if (string.IsNullOrEmpty(sucursal.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", sucursal.senas);
                if (string.IsNullOrEmpty(sucursal.descripcion))
                    comando.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Descripcion", sucursal.descripcion);
                comando.Parameters.AddWithValue("@Nombre", sucursal.nombre);
                comando.Parameters.AddWithValue("@Compania", sucursal.compania);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si una sucursal esta borrada (para cuando se quiere crear una sucursal)
        /// </summary>
        /// <param name="nombre">nombre de la sucursal a buscar</param>
        /// <returns>true si la sucursal existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Nombre] FROM[dbo].[SUCURSAL] " +
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
        /// Consulta si una sucursal aun tiene pedidos
        /// </summary>
        /// <param name="nombre">nombre de la sucursal a buscar</param>
        /// <returns>true si la sucursal se puede borrar
        /// false en caso contrario</returns>
        public static bool ConsultarParaBorrarPorPedido(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Numero] FROM[dbo].[PEDIDO] " +
                "WHERE Activo=1 AND Sucursal= @nombre AND (Estado = 'n' OR Estado = 'p' OR Estado = 'F')";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                var reader = comando.ExecuteReader();
                //No se encuentra el rol
                if (!reader.HasRows) { conexion.Close(); return true; }
                //Si se encontro el rol
                conexion.Close();
                return false;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si una sucursal aun tiene empleados
        /// </summary>
        /// <param name="nombre">nombre de la sucursal a buscar</param>
        /// <returns>true si la sucursal se puede borrar
        /// false en caso contrario</returns>
        public static bool ConsultarParaBorrarPorEmpleado(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Cedula] FROM[dbo].[EMPLEADO] " +
                "WHERE Activo=1 AND Sucursal= @nombre";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                var reader = comando.ExecuteReader();
                //No se encuentra el rol
                if (!reader.HasRows) { conexion.Close(); return true; }
                //Si se encontro el rol
                conexion.Close();
                return false;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza el borrado logico de los pedidos de una sucursal que se borró
        /// </summary>
        /// <param name="nombre">nombre de la sucursal que se borro</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarPedidos(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [ACTIVO]=0" +
                "WHERE Sucursal= @sucursal";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@sucursal", nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza el borrado logico de la administracion de una sucursal que se borró
        /// </summary>
        /// <param name="nombre">nombre de la sucursal que se borro</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarAdmin(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[ADMINISTRADORXSUCURSAL] SET [ACTIVO]=0" +
                "WHERE Sucursal= @sucursal";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@sucursal", nombre);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
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