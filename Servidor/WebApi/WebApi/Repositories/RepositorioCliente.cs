using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con los clientes 
    /// </summary>
    public class RepositorioCliente
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------//
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
                "WHERE Activo=1 AND Cedula= @cedula AND CONVERT(VARCHAR(300)," +
                "DECRYPTBYPASSPHRASE('GaStFa3RB', Contrasena))= @contrasena";
            //ejecuta el query
            try
            {
                conexion.Close();
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

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los clientes de la base de datos
        /// </summary>
        /// <returns>lista con la informacion de todos los clientes</returns>
        public static List<ClienteRecibo> ConsultarClientes()
        {
            //Lista que almacena los clientes
            var lista = new List<ClienteRecibo>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Senas], [FechaNacimiento], [Prioridad] FROM[dbo].[CLIENTE] WHERE Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new ClienteRecibo
                        {
                            cedula = GetValue<int>(reader["cedula"]),
                            nombre1 = GetValue<string>(reader["nombre1"]),
                            nombre2 = GetValue<string>(reader["nombre2"]),
                            apellido1 = GetValue<string>(reader["apellido1"]),
                            apellido2 = GetValue<string>(reader["apellido2"]),
                            provincia = GetValue<string>(reader["provincia"]),
                            ciudad = GetValue<string>(reader["cuidad"]),
                            senas = GetValue<string>(reader["senas"]),
                            fechaNacimiento = GetValue<string>(reader["fechaNacimiento"]),
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
        /// Crea un cliente nuevo
        /// </summary>
        /// <param name="cliente">Modelo de cliente a insertar</param>
        /// <returns>true si se pudo realizar la insercion / false en caso contrario</returns>
        public static bool AgregarCliente(ClienteRecibo cliente)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[CLIENTE] ([Cedula] ,[Nombre1], [Nombre2] ,[Apellido1], " +
                "[Apellido2] ,[Provincia] ,[Cuidad] ,[Senas] ,[FechaNacimiento] ,[Prioridad] , " +
                "[Contrasena] ,[Activo]) VALUES(@Cedula, @Nombre1, @Nombre2, @Apellido1," +
                "@Apellido2, @Provincia, @Cuidad, @Senas, @FechaNacimiento, @Prioridad, " +
                "ENCRYPTBYPASSPHRASE ('GaStFa3RB',@Contraseña), @Activo)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cedula", cliente.cedula);
                comando.Parameters.AddWithValue("@Contraseña", cliente.contrasena);
                comando.Parameters.AddWithValue("@Nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@Apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@Provincia", cliente.provincia);
                if (string.IsNullOrEmpty(cliente.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Nombre2", cliente.nombre2);
                if (string.IsNullOrEmpty(cliente.apellido2))
                    comando.Parameters.AddWithValue("@Apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Apellido2", cliente.apellido2);
                if (string.IsNullOrEmpty(cliente.ciudad))
                    comando.Parameters.AddWithValue("@Cuidad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Cuidad", cliente.ciudad);
                if (string.IsNullOrEmpty(cliente.senas))
                    comando.Parameters.AddWithValue("@Senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@Senas", cliente.senas);
                comando.Parameters.AddWithValue("@FechaNacimiento", cliente.fechaNacimiento);
                comando.Parameters.AddWithValue("@Prioridad", 1);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                try { 
                    AgregarTelefonos(cliente);
                    AgregarPadecimientos(cliente);
                }
                catch (Exception) { return true; }
                return true;
            }
            catch (Exception){return false;}
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza el borrado logico de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarCliente(int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[CLIENTE] SET [ACTIVO]=0" +
                "WHERE Cedula= @cedula AND Activo=1";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                BorrarPadecimientos(cedula);
                BorrarTelefonos(cedula);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Realiza el borrado logico de los telefonos de un cliente eliminado
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar los telefonos</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarTelefonos(int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[TELEFONOXCLIENTE] SET [ACTIVO]=0" +
                "WHERE Cliente= @cedula";
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
        /// Realiza el borrado logico de los padecimientos de un cliente eliminado
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar los telefonos</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarPadecimientos(int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PADECIMIENTO] SET [ACTIVO]=0" +
                "WHERE Cliente= @cedula";
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
        /// Actualiza la contraseña de un cliente
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar la contraseña</param>
        /// <param name="contrasenaActual">contraseña actual del cliente</param>
        /// <param name="contrasenaNueva">nueva contraseña del cliente</param>
        /// <returns>true si se actualiza la contraseña / false en caso contrario</returns>
        public static bool ActualizarContraseña(int cedula, string contrasenaActual, string contrasenaNueva)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[CLIENTE] SET [Contrasena]=ENCRYPTBYPASSPHRASE ('GaStFa3RB',@nueva) " +
                "WHERE Cedula= @cedula AND CONVERT(VARCHAR(300), DECRYPTBYPASSPHRASE('GaStFa3RB', Contrasena)) = @vieja";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@nueva", contrasenaNueva);
                comando.Parameters.AddWithValue("@vieja", contrasenaActual);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (reader.RecordsAffected<1) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { conexion.Close(); return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega los telefonos del cliente
        /// </summary>
        /// <param name="cliente">informacion del cliente</param>
        /// <returns>true si agrega los telefonos / false en caso contrario</returns>
        public static bool AgregarTelefonos(ClienteRecibo cliente)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            String[] telefonos = js.Deserialize<String[]>(cliente.telefonos);
            foreach (string telefono in telefonos)//Por cada telefono 
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[TELEFONOXCLIENTE] ([Cliente] ,[Telefono], [Activo]) " +
                    " VALUES(@Cliente, @Telefono, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Cliente", cliente.cedula);
                    comando.Parameters.AddWithValue("@Telefono", int.Parse(telefono));
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                }
                catch (Exception) { return false; }
            }
            return true;
        }
        
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega los padecimientos del cliente
        /// </summary>
        /// <param name="cliente">informacion cliente</param>
        /// <returns>true si agrega los padecimientos / false en caso contrario</returns>
        public static bool AgregarPadecimientos(ClienteRecibo cliente)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Padecimiento[] padecimientos = js.Deserialize<Padecimiento[]>(cliente.padecimientos);
            foreach (Padecimiento padecimiento in padecimientos)//Por cada padecimiento
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[PADECIMIENTO] ([Cliente] ,[Padecimiento], [Año], [Activo]) " +
                    " VALUES(@Cliente, @Padecimiento, @Año, @Activo)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Cliente", cliente.cedula);
                    comando.Parameters.AddWithValue("@Padecimiento",padecimiento.padecimiento);
                    if (string.IsNullOrEmpty(padecimiento.año.ToString()) || padecimiento.año.ToString()=="0")
                        comando.Parameters.AddWithValue("@Año", DBNull.Value);
                    else
                        comando.Parameters.AddWithValue("@Año", padecimiento.año);
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                }
                catch (Exception) { return false; }
            }
            return true;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los padecimientos de un cliente
        /// </summary>
        /// <param name="cedula">cliente a consultar los padecimientos</param>
        /// <returns>lista de padecimientos del cliente</returns>
        public static List<Padecimiento> ConsultarPadecimientos(int cedula)
        {
            var lista = new List<Padecimiento>();
            //Query que consulta la base de datos
            var query = "SELECT [Padecimiento], [Año] FROM[dbo].[PADECIMIENTO] " +
                "WHERE Activo=1 AND Cliente=@Cliente";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cliente", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Padecimiento
                        {padecimiento = GetValue<string>(reader["padecimiento"]),
                            año = GetValue<int>(reader["año"])
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los telefonos del cliente 
        /// </summary>
        /// <param name="cedula">cliente a consultar</param>
        /// <returns>lista con los telefonos</returns>
        public static List<string> ConsultarTelefonos(int cedula)
        {
            var lista = new List<String>();
            //Query que consulta la base de datos
            var query = "SELECT [Telefono] FROM[dbo].[TELEFONOXCLIENTE] " +
                "WHERE Activo=1 AND Cliente=@Cliente";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Cliente", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(GetValue<string>(reader["telefono"]));
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta toda la informacion de un cliente incluido padecimientos y telefonos 
        /// </summary>
        /// <param name="cedula">cliente a consultar</param>
        /// <returns>Cliente con toda la informacion</returns>
        public static List<ClienteEnvio> ConsultarModClientes(int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<ClienteEnvio>();
            //Query que consulta la base de datos
            var query = "SELECT [Cedula], [Nombre1], [Nombre2], [Apellido1], [Apellido2], [Provincia]," +
                "[Cuidad], [Senas], [FechaNacimiento], [Prioridad] FROM[dbo].[CLIENTE] WHERE Activo=1 " +
                "AND Cedula=@cedula";
            //Se ejecuta el query
            try
            {
                List<Padecimiento> padecimientos = ConsultarPadecimientos(cedula);
                List<string> telefonos = ConsultarTelefonos(cedula);
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                var reader = comando.ExecuteReader();
                reader.Read();
                lista.Add(new ClienteEnvio
                {
                    cedula = GetValue<int>(reader["cedula"]),
                    nombre1 = GetValue<string>(reader["nombre1"]),
                    nombre2 = GetValue<string>(reader["nombre2"]),
                    apellido1 = GetValue<string>(reader["apellido1"]),
                    apellido2 = GetValue<string>(reader["apellido2"]),
                    provincia = GetValue<string>(reader["provincia"]),
                    ciudad = GetValue<string>(reader["cuidad"]),
                    senas = GetValue<string>(reader["senas"]),
                    fechaNacimiento = GetValue<string>(reader["fechaNacimiento"]),
                    contrasena = "",
                    telefonos = telefonos,
                    padecimientos = padecimientos
                });
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra los telefonos de los clientes para luego reinsertalos modificados
        /// </summary>
        /// <param name="cedula">cliente a borrar los telefonos</param>
        /// <returns>true si se pudo borrar / false en caso contrario</returns>
        public static bool BorradoFisicoTelefono(int cedula)
        {
            //query de la solicitud
            var query = "DELETE FROM [dbo].[TELEFONOXCLIENTE] "+
                "WHERE Cliente= @cedula";
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
        /// Borra los padecimientos de los clientes para luego reinsertalos modificados
        /// </summary>
        /// <param name="cedula">cliente a borrar los telefonos</param>
        /// <returns>true si se pudo borrar / false en caso contrario</returns>
        public static bool BorradoFisicoPadecimiento(int cedula)
        {
            //query de la solicitud
            var query = "DELETE FROM [dbo].[PADECIMIENTO] " +
                "WHERE Cliente= @cedula";
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
        /// Actualiza la informacion de los clientes
        /// </summary>
        /// <param name="cliente">informacion a actualizar del cliente</param>
        /// <returns>true si se actualizo / false en caso contrario</returns>
        public static bool ActualizarCliente(ClienteRecibo cliente)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[CLIENTE] SET [Activo] = 1, [Nombre1] = @nombre1, [Nombre2]=@nombre2, " +
                "[Apellido1]=@apellido1, [Apellido2]=@apellido2, [Provincia]=@provincia," +
                "[Cuidad]=@ciudad ,[Senas]=@senas ,[FechaNacimiento]=@fecha " +
                "WHERE Cedula=@cedula";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula",cliente.cedula);
                comando.Parameters.AddWithValue("@nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@provincia", cliente.provincia);
                comando.Parameters.AddWithValue("@fecha", cliente.fechaNacimiento);
                if (string.IsNullOrEmpty(cliente.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@nombre2", cliente.nombre2);
                if (string.IsNullOrEmpty(cliente.apellido2))
                    comando.Parameters.AddWithValue("@apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@apellido2", cliente.apellido2);
                if (string.IsNullOrEmpty(cliente.ciudad))
                    comando.Parameters.AddWithValue("@ciudad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@ciudad", cliente.ciudad);
                if (string.IsNullOrEmpty(cliente.senas))
                    comando.Parameters.AddWithValue("@senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@senas", cliente.senas);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                //Borra y reinserta los padecimientos y telefonos actualizados de los clientes
                if (BorradoFisicoTelefono(cliente.cedula) && AgregarTelefonos(cliente))
                {
                    if (BorradoFisicoPadecimiento(cliente.cedula) && AgregarPadecimientos(cliente)) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de los clientes
        /// </summary>
        /// <param name="cliente">informacion a actualizar del cliente</param>
        /// <returns>true si se actualizo / false en caso contrario</returns>
        public static bool ActualizarClienteAdmin(ClienteRecibo cliente)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[CLIENTE] SET [Activo] = 1, [Nombre1] = @nombre1, [Nombre2]=@nombre2, " +
                "[Apellido1]=@apellido1, [Apellido2]=@apellido2, [Provincia]=@provincia," +
                "[Cuidad]=@ciudad ,[Senas]=@senas ,[FechaNacimiento]=@fecha " +
                "WHERE Cedula=@cedula";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cliente.cedula);
                comando.Parameters.AddWithValue("@nombre1", cliente.nombre1);
                comando.Parameters.AddWithValue("@apellido1", cliente.apellido1);
                comando.Parameters.AddWithValue("@provincia", cliente.provincia);
                comando.Parameters.AddWithValue("@fecha", cliente.fechaNacimiento);
                if (string.IsNullOrEmpty(cliente.nombre2))
                    comando.Parameters.AddWithValue("@Nombre2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@nombre2", cliente.nombre2);
                if (string.IsNullOrEmpty(cliente.apellido2))
                    comando.Parameters.AddWithValue("@apellido2", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@apellido2", cliente.apellido2);
                if (string.IsNullOrEmpty(cliente.ciudad))
                    comando.Parameters.AddWithValue("@ciudad", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@ciudad", cliente.ciudad);
                if (string.IsNullOrEmpty(cliente.senas))
                    comando.Parameters.AddWithValue("@senas", DBNull.Value);
                else
                    comando.Parameters.AddWithValue("@senas", cliente.senas);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un cliente esta borrado (para cuando se quiere registrar)
        /// </summary>
        /// <param name="cedula">cedula del cliente ingresada</param>
        /// <returns>true si el cliente existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(int cedula)
        {
            //query de la solicitud
            var query = "SELECT [Nombre1], [Apellido1] FROM[dbo].[CLIENTE] " +
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
        /// Actualiza la contraseña de un cliente (cuando se registra luego de haberse borrado)
        /// </summary>
        /// <param name="cedula">cedula del cliente a borrar la contraseña</param>
        /// <param name="contrasenaActual">contraseña actual del cliente</param>
        /// <param name="contrasenaNueva">nueva contraseña del cliente</param>
        /// <returns>true si se actualiza la contraseña / false en caso contrario</returns>
        public static bool AgregarContraseña(int cedula, string contrasenaNueva)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[CLIENTE] SET [Contrasena]=ENCRYPTBYPASSPHRASE ('GaStFa3RB',@nueva) " +
                "WHERE Cedula= @cedula";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@nueva", contrasenaNueva);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (reader.RecordsAffected < 1) { conexion.Close(); return false; }
                //Si se encontro el cliente
                conexion.Close();
                return true;
            }
            catch (Exception) { conexion.Close(); return false; }
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