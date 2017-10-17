using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de empleados
    /// </summary>
    public class RepositorioEmpleado
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Valida el logeo de un empleado, y obtiene sus roles
        /// </summary>
        /// <param name="cedula">cedula del empleado que intenta logearse</param>
        /// <param name="contrasena">contrasena del empleado que intenta logearse</param>
        /// <returns>Lista de los roles del empleado si existe</returns>
        public static List<string> LogearEmpleado(int cedula, string contrasena)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1], [Apellido1] FROM[dbo].[EMPLEADO] " +
                "WHERE Cedula= @cedula AND CONVERT(VARCHAR(300)," +
                "DECRYPTBYPASSPHRASE('GaStFa3RB', Contraseña))=@contrasena AND Activo=1 AND " +
                "(Rol = 'Administrador' OR Rol = 'FARMACEUTICO')";
            //ejecuta el query
            try
            {
                conexion.Close();
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

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los roles de un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado a consultar los roles</param>
        /// <returns>lista de roles del empleado</returns>
        public static List<string> consultarRoles(int cedula)
        {
            var lista = new List<string>();
            //query de la solicitud
            var query = "SELECT [Rol] FROM[dbo].[EMPLEADO] " +
                "WHERE Cedula= @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(GetValue<string>(reader["rol"]));
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta la compañia de un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado a consultar la compañia</param>
        /// <returns>lista de roles del empleado</returns>
        public static ObjGeneral consultarCompañia(int cedula)
        {
            //query de la solicitud
            var query = "Select Compañia, SUCURSAL.Nombre as sucursal FROM SUCURSAL INNER JOIN EMPLEADO " +
                "ON EMPLEADO.Sucursal = Sucursal.Nombre WHERE EMPLEADO.Cedula = @cedula";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                var reader = comando.ExecuteReader();
                reader.Read();
                var info = new ObjGeneral { opcion = GetValue<string>(reader["compañia"]),
                    opcion2 = GetValue<string>(reader["sucursal"])};
                conexion.Close();
                return info;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Crea un empleado nuevo
        /// </summary>
        /// <param name="empleado">Modelo de empleado a insertar</param>
        /// <returns>true si se pudo realizar la insercion / false en caso contrario</returns>
        public static bool AgregarEmpleado(EmpleadoRecibo empleado)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[Empleado] ([Cedula] ,[Nombre1], [Nombre2] ,[Apellido1], " +
                "[Apellido2] ,[Provincia] ,[Cuidad] ,[Señas] ,[FechaNacimiento] ,[Sucursal] , " +
                "[Rol], [Activo], [Contraseña]) VALUES(@Cedula, @Nombre1, @Nombre2, @Apellido1, " +
                "@Apellido2, @Provincia, @Cuidad, @Senas, @FechaNacimiento, @Sucursal, @Rol, @Activo, " +
                "ENCRYPTBYPASSPHRASE ('GaStFa3RB',@Contraseña))";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", empleado.cedula);
                comando.Parameters.AddWithValue("@Contraseña", empleado.contrasena);
                comando.Parameters.AddWithValue("@Nombre1", empleado.nombre1);
                comando.Parameters.AddWithValue("@Apellido1", empleado.apellido1);
                comando.Parameters.AddWithValue("@Provincia", empleado.provincia);
                if (string.IsNullOrEmpty(empleado.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Nombre2", empleado.nombre2);
                if (string.IsNullOrEmpty(empleado.apellido2))
                    comando.Parameters.AddWithValue("@Apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Apellido2", empleado.apellido2);
                if (string.IsNullOrEmpty(empleado.ciudad))
                    comando.Parameters.AddWithValue("@Cuidad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Cuidad", empleado.ciudad);
                if (string.IsNullOrEmpty(empleado.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", empleado.senas);
                comando.Parameters.AddWithValue("@FechaNacimiento", empleado.fechaNacimiento);
                comando.Parameters.AddWithValue("@Sucursal", empleado.sucursal);
                comando.Parameters.AddWithValue("@Rol", empleado.rol);
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
        /// Consulta la informacion de todos los empleados
        /// </summary>
        /// <returns>Lista con la informacion de todos los empleados</returns>
        public static List<EmpleadoRecibo> ConsultarEmpleados(string compania)
        {
            //Lista que almacena los empleado
            var lista = new List<EmpleadoRecibo>();
            //Query que consulta la base de datos
            var query = "SELECT EMPLEADO.[Cedula], EMPLEADO.[Nombre1], EMPLEADO.[Nombre2], " +
                "EMPLEADO.[Apellido1], EMPLEADO.[Apellido2], EMPLEADO.[Provincia], " +
                "EMPLEADO.[Cuidad], EMPLEADO.[Señas], EMPLEADO.[FechaNacimiento], EMPLEADO.[Sucursal], " +
                "EMPLEADO.[Rol] FROM[dbo].[EMPLEADO] INNER JOIN SUCURSAL " +
                "ON EMPLEADO.Sucursal = Sucursal.Nombre WHERE EMPLEADO.Activo=1 AND Sucursal.Compañia=@compania";

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
                        lista.Add(new EmpleadoRecibo
                        {
                            cedula = GetValue<int>(reader["cedula"]),
                            nombre1 = GetValue<string>(reader["nombre1"]),
                            nombre2 = GetValue<string>(reader["nombre2"]),
                            apellido1 = GetValue<string>(reader["apellido1"]),
                            apellido2 = GetValue<string>(reader["apellido2"]),
                            provincia = GetValue<string>(reader["provincia"]),
                            ciudad = GetValue<string>(reader["cuidad"]),
                            senas = GetValue<string>(reader["señas"]),
                            fechaNacimiento = GetValue<string>(reader["fechaNacimiento"]),
                            sucursal = GetValue<string>(reader["sucursal"]),
                            rol = GetValue<string>(reader["rol"]),
                            contrasena = ""
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado a borrar</param>
        /// <returns>true si pudo borrar el empleado/false en caso contrario</returns>
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

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza un empleado
        /// </summary>
        /// <param name="empleado">Modelo de empleado a actualizar</param>
        /// <returns>true si se pudo realizar la insercion / false en caso contrario</returns>
        public static bool ActualizarEmpleado(EmpleadoRecibo empleado)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[Empleado] SET [Nombre1]=@Nombre1, [Nombre2]=@Nombre2, " +
                "[Apellido1]=@Apellido1, [Apellido2]=@Apellido2, [Provincia]=@Provincia, " +
                "[Cuidad]=@Cuidad ,[Señas]=@Senas, [FechaNacimiento]=@FechaNacimiento, " +
                "[Sucursal]=@Sucursal, [Rol]=@Rol WHERE Cedula=@Cedula";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", empleado.cedula);
                comando.Parameters.AddWithValue("@Nombre1", empleado.nombre1);
                comando.Parameters.AddWithValue("@Apellido1", empleado.apellido1);
                comando.Parameters.AddWithValue("@Provincia", empleado.provincia);
                if (string.IsNullOrEmpty(empleado.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Nombre2", empleado.nombre2);
                if (string.IsNullOrEmpty(empleado.apellido2))
                    comando.Parameters.AddWithValue("@Apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Apellido2", empleado.apellido2);
                if (string.IsNullOrEmpty(empleado.ciudad))
                    comando.Parameters.AddWithValue("@Cuidad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Cuidad", empleado.ciudad);
                if (string.IsNullOrEmpty(empleado.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", empleado.senas);
                comando.Parameters.AddWithValue("@FechaNacimiento", empleado.fechaNacimiento);
                comando.Parameters.AddWithValue("@Sucursal", empleado.sucursal);
                comando.Parameters.AddWithValue("@Rol", empleado.rol);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza un empleado
        /// </summary>
        /// <param name="empleado">Modelo de empleado a actualizar</param>
        /// <returns>true si se pudo realizar la insercion / false en caso contrario</returns>
        public static bool InsertarEmpleadoBorrado(EmpleadoRecibo empleado)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[Empleado] SET [Nombre1]=@Nombre1, [Nombre2]=@Nombre2, " +
                "[Apellido1]=@Apellido1, [Apellido2]=@Apellido2, [Provincia]=@Provincia, " +
                "[Cuidad]=@Cuidad ,[Señas]=@Senas, [FechaNacimiento]=@FechaNacimiento, " +
                "[Sucursal]=@Sucursal, [Rol]=@Rol, [Activo]=1, " +
                "[Contraseña]=ENCRYPTBYPASSPHRASE ('GaStFa3RB',@Contraseña)" +
                " WHERE Cedula=@Cedula";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", empleado.cedula);
                comando.Parameters.AddWithValue("@Nombre1", empleado.nombre1);
                comando.Parameters.AddWithValue("@Apellido1", empleado.apellido1);
                comando.Parameters.AddWithValue("@Provincia", empleado.provincia);
                comando.Parameters.AddWithValue("@Contraseña", empleado.contrasena);
                if (string.IsNullOrEmpty(empleado.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Nombre2", empleado.nombre2);
                if (string.IsNullOrEmpty(empleado.apellido2))
                    comando.Parameters.AddWithValue("@Apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Apellido2", empleado.apellido2);
                if (string.IsNullOrEmpty(empleado.ciudad))
                    comando.Parameters.AddWithValue("@Cuidad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Cuidad", empleado.ciudad);
                if (string.IsNullOrEmpty(empleado.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", empleado.senas);
                comando.Parameters.AddWithValue("@FechaNacimiento", empleado.fechaNacimiento);
                comando.Parameters.AddWithValue("@Sucursal", empleado.sucursal);
                comando.Parameters.AddWithValue("@Rol", empleado.rol);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un empleado esta borrado (para cuando se quiere registrar)
        /// </summary>
        /// <param name="cedula">cedula del empleado ingresada</param>
        /// <returns>true si el cliente existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(int cedula)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1] FROM[dbo].[EMPLEADO] " +
                "WHERE Activo=0 AND Cedula= @cedula";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (!reader.HasRows) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un empleado es administrador
        /// </summary>
        /// <param name="cedula">cedula del empleado ingresada</param>
        /// <returns>true si el cliente existe
        /// false en caso contrario</returns>
        public static bool ConsultarAdmin(int cedula)
        {
            //query de la solicitud
            var query = "SELECT [Sucursal] FROM[dbo].[ADMINISTRADORXSUCURSAL] " +
                "WHERE Activo=1 AND Administrador= @cedula";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (!reader.HasRows) { conexion.Close(); return true; }
                //Si se encontro el cliente
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