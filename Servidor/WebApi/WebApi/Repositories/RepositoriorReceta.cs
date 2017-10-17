using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de recetas 
    /// </summary>
    public class RepositorioReceta
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta todos los nombres y numeros de las recetas de un cliente
        /// </summary>
        /// <param name="cedula">cliente a consultar la receta</param>
        /// <returns></returns>
        public static List<ObjGeneral> ConsultarRecetas(int cedula)
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero]" +
                " FROM[dbo].[RECETA] WHERE Activo=1 AND Cliente= @cliente";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        lista.Add(new ObjGeneral
                        {
                            opcion = GetValue<string>(reader["nombre"]),
                            opcion2 = GetValue<string>(reader["numero"].ToString())
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega una nueva receta
        /// </summary>
        /// <param name="receta">informacion de la receta a insertar</param>
        /// <param name="cliente">cliente que quiere agregar la receta</param>
        /// <returns>true si se inserto correctamente / false en caso contrario</returns>
        public static bool AgregarReceta(RecetaRecibo receta, int cliente)
        {
            byte[] imageBytes = Convert.FromBase64String(receta.imagen);

            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[RECETA] ([Imagen], [Nombre], [Numero], " +
                "[Activo], [Cliente], [Usado], [Doctor]) " +
                "VALUES(@Imagen, @Nombre, @Numero, @Activo, @Cliente, @Usado, @Doctor)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Imagen", imageBytes);
                comando.Parameters.AddWithValue("@Nombre", receta.nombre);
                comando.Parameters.AddWithValue("@Numero", receta.numero);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.Parameters.AddWithValue("@Cliente", cliente);
                comando.Parameters.AddWithValue("@Usado", 0);
                comando.Parameters.AddWithValue("@Doctor", receta.doctor);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                AgregarMedicamentosReceta(receta);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borrado logico de una receta
        /// </summary>
        /// <param name="receta">receta a borrar</param>
        /// <param name="cedula">cliente que quiere borrar la receta</param>
        /// <returns>true si se pudo borrar /false en caso contrario</returns>
        public static bool BorrarReceta(int receta, int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[RECETA] SET [ACTIVO]=0" +
                "WHERE Numero= @numero AND Cliente =@cliente";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                comando.Parameters.AddWithValue("@numero", receta);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                BorrarMedicamentos(receta);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta toda la informacion de una receta
        /// </summary>
        /// <param name="numero">numero de receta a consultar</param>
        /// <param name="cedula">cliente que la consulta</param>
        /// <returns>Lista con toda la informacion de la receta</returns>
        public static List<RecetaEnvio> ConsultarReceta(int numero, int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<RecetaEnvio>();
            //Query que consulta la base de datos
            var query = "SELECT [Numero], [Nombre], [Imagen], [Doctor]" +
                " FROM[dbo].[RECETA] WHERE Activo=1 AND Cliente= @cliente AND Numero= @numero ";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                comando.Parameters.AddWithValue("@numero", numero);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        List<MedicamentoReceta> productos = ConsultarMedicamentosReceta(reader.GetInt32(0));
                        lista.Add(new RecetaEnvio
                        {
                            nombre = GetValue<string>(reader["nombre"]),
                            imagen = Convert.ToBase64String((byte[])(reader[2])),
                            productos = productos,
                            numero = GetValue<int>(reader["numero"]),
                            doctor = GetValue<string>(reader["doctor"])
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los medicamentos de una receta
        /// </summary>
        /// <param name="receta">numero de receta a consultar</param>
        /// <returns>lista con la informacion de los medicamentos de la receta</returns>
        public static List<MedicamentoReceta> ConsultarMedicamentosReceta(int receta)
        {
            String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
            SqlConnection conexion = new SqlConnection(conexionString);
            //Lista que almacena los clientes
            var lista = new List<MedicamentoReceta>();
            //Query que consulta la base de datos
            var query = "SELECT [Medicamento], [MEDICAMENTOXRECETA].Cantidad" +
                " FROM[dbo].[MEDICAMENTOXRECETA] WHERE Activo=1 AND Receta= @receta";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@receta", receta);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new MedicamentoReceta
                        {
                            medicamento = reader.GetString(0),
                            cantidad = reader.GetInt32(1)
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega los medicamentos de una receta
        /// </summary>
        /// <param name="receta">informacion del la receta</param>
        /// <returns>true si agrega los telefonos / false en caso contrario</returns>
        public static bool AgregarMedicamentosReceta(RecetaRecibo receta)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            MedicamentoReceta[] medicamentos = js.Deserialize<MedicamentoReceta[]>(receta.medicamentos);
            foreach (MedicamentoReceta medicamento in medicamentos)//Por cada telefono 
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[MEDICAMENTOXRECETA] ([Receta] ,[Medicamento], " +
                    "[Activo], [Cantidad]) " +
                    " VALUES(@Receta, @Medicamento, @Activo, @Cantidad)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Receta", receta.numero);
                    comando.Parameters.AddWithValue("@Medicamento", medicamento.medicamento);
                    comando.Parameters.AddWithValue("@Activo", 1);
                    comando.Parameters.AddWithValue("@Cantidad", medicamento.cantidad);
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
        /// Borra los medicamentos de una receta para luego reinsertalos modificados
        /// </summary>
        /// <param name="cedula">cliente a borrar los telefonos</param>
        /// <returns>true si se pudo borrar / false en caso contrario</returns>
        public static bool BorradoFisicoMedicamentosReceta(int numero)
        {
            //query de la solicitud
            var query = "DELETE FROM [dbo].[MEDICAMENTOXRECETA] " +
                "WHERE Receta= @numero";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si se puede borrar una receta (Si la receta esta unida a un pedido 
        /// que no se ha preparado no se puede borrar)
        /// </summary>
        /// <param name="numero">numero de la receta a borrra</param>
        /// <returns>true si se puede borrar
        /// false en caso contrario</returns>
        public static bool ConsultarParaBorrar(int numero)
        {
            //query de la solicitud
            var query = "SELECT RECETA.Nombre FROM RECETA INNER JOIN MEDICAMENTOXPEDIDO " +
                "ON RECETA.Numero = MEDICAMENTOXPEDIDO.Receta INNER JOIN PEDIDO " +
                "ON PEDIDO.Numero = MEDICAMENTOXPEDIDO.Pedido WHERE RECETA.Numero = @Numero " +
                "AND PEDIDO.ESTADO = 'n'";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Numero", numero);
                var reader = comando.ExecuteReader();
                //No se encuentra el cliente
                if (!reader.HasRows) { conexion.Close(); return true; }
                //Si se encontro el cliente
                conexion.Close();
                return false;
            }
            catch (Exception) { return true; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si una receta esta borrada (para cuando se quiere volver a crear)
        /// </summary>
        /// <param name="numero">numero de receta a crear</param>
        /// <returns>true si la receta existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(int numero)
        {
            //query de la solicitud
            var query = "SELECT [Nombre] FROM[dbo].[RECETA] " +
                "WHERE Activo=0 AND Numero= @numero AND Usado = 0";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
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
        /// Consulta si una receta ya fue usada
        /// </summary>
        /// <param name="numero">numero de receta a crear</param>
        /// <returns>true si la receta existe
        /// false en caso contrario</returns>
        public static bool ConsultarUso(int numero)
        {
            //query de la solicitud
            var query = "SELECT [Nombre] FROM[dbo].[RECETA] " +
                "WHERE Activo=1 AND Numero= @numero AND Usado = 0";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
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
        /// Realiza el borrado logico de los medicamentos de una receta eliminada
        /// </summary>
        /// <param name="numero">receta a borrar los medicamentos</param>
        /// <returns>true si se borra / false en caso contrario</returns>
        public static bool BorrarMedicamentos(int numero)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[MEDICAMENTOXRECETA] SET [ACTIVO]=0" +
                "WHERE Receta= @numero";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza la informacion de una receta
        /// </summary>
        /// <param name="receta">informacion a actualizar de la receta</param>
        /// <returns>true si se actualizo / false en caso contrario</returns>
        public static bool ActualizarReceta(RecetaRecibo receta, int cliente)
        {
            byte[] imageBytes = Convert.FromBase64String(receta.imagen);
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[RECETA] SET [Activo] = 1, [Nombre] = @nombre, " +
                "[Numero]=@numero, [Imagen]=@imagen, [Doctor]=@doctor," +
                "[Cliente]=@cliente ,[Usado]=0 " +
                "WHERE Numero=@numero AND Usado=0";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Imagen", imageBytes);
                comando.Parameters.AddWithValue("@Nombre", receta.nombre);
                comando.Parameters.AddWithValue("@Numero", receta.numero);
                comando.Parameters.AddWithValue("@Cliente", cliente);
                comando.Parameters.AddWithValue("@Doctor", receta.doctor);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                //Borra y reinserta los medicamentos de la receta
                if (BorradoFisicoMedicamentosReceta(receta.numero) && AgregarMedicamentosReceta(receta)) { return true; }
                else { return false; }
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