using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de clientes 
    /// </summary>
    public class RepositorioMedicamento
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS;" +
            " Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los medicamentos
        /// </summary>
        /// <returns>Lista con la informacion de todos los medicamentos</returns>
        public static List<Medicamento> ConsultarMedicamento()
        {
            //Lista que almacena los client
            var lista = new List<Medicamento>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Precio], [Prescripcion], [Cantidad], [CasaFarmaceutica]" +
                " FROM[dbo].[MEDICAMENTO] WHERE Activo=1";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new Medicamento {
                            nombre = GetValue<string>(reader["nombre"]),
                            precio = GetValue<int>(reader["precio"]),
                            prescripcion = GetValue<bool>(reader["Prescripcion"]),
                            cantidad = GetValue<int>(reader["cantidad"]),
                            casaFarmaceutica = GetValue<string>(reader["CasaFarmaceutica"])
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega un medicamento nuevo
        /// </summary>
        /// <param name="medicamento">medicamento a agregar</param>
        /// <returns>true si el medicamento se crea / false en caso contrario</returns>
        public static bool AgregarMedicamento(Medicamento medicamento)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[MEDICAMENTO] ([Nombre] ,[Prescripcion], [Cantidad], " +
                "[CasaFarmaceutica], [Activo] ,[Precio]) VALUES(@Nombre, @Prescripcion," +
                " @Cantidad, @CasaFarmaceutica, @Activo, @Precio)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Nombre", medicamento.nombre);
                comando.Parameters.AddWithValue("@Prescripcion", medicamento.prescripcion);
                comando.Parameters.AddWithValue("@Cantidad", medicamento.cantidad);
                comando.Parameters.AddWithValue("@CasaFarmaceutica", medicamento.casaFarmaceutica);
                comando.Parameters.AddWithValue("@Activo", 1);
                comando.Parameters.AddWithValue("@Precio", medicamento.precio);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza un medicamento
        /// </summary>
        /// <param name="medicamento"> medicamento a actualizar</param>
        /// <returns></returns>
        public static bool ActualizarMedicamento(Medicamento medicamento)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[MEDICAMENTO] SET [Prescripcion] = @Prescripcion, [Cantidad]=@Cantidad, " +
                "[Precio]=@Precio, [CasaFarmaceutica]=@CasaFarmaceutica, [Activo]=1 " +
                "WHERE Nombre=@Nombre";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Nombre", medicamento.nombre);
                comando.Parameters.AddWithValue("@Prescripcion", medicamento.prescripcion);
                comando.Parameters.AddWithValue("@Cantidad", medicamento.cantidad);
                comando.Parameters.AddWithValue("@CasaFarmaceutica", medicamento.casaFarmaceutica);
                comando.Parameters.AddWithValue("@Precio", medicamento.precio);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un medicamento
        /// </summary>
        /// <param name="nombre">medicamento a borrar</param>
        /// <returns>true si borra el medicamento / false en caso contrario</returns>
        public static bool BorrarMedicamento(string nombre)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[MEDICAMENTO] SET [ACTIVO]=0" +
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

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un medicamento aun se encuentra en un pedido
        /// </summary>
        /// <param name="nombre">nombre del medicamento a buscar</param>
        /// <returns>true si el medicamento se puede borrar
        /// false en caso contrario</returns>
        public static bool ConsultarParaBorrarPorPedido(string nombre)
        {
            //query de la solicitud
            var query = "SELECT Pedido FROM[dbo].[MEDICAMENTOXPEDIDO] " +
                "INNER JOIN [PEDIDO] ON [PEDIDO].Numero=[MEDICAMENTOXPEDIDO].PEDIDO " +
                "WHERE [MEDICAMENTOXPEDIDO].Medicamento= @nombre AND (Estado = 'n' OR Estado = 'p' OR Estado = 'F')";
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
        /// Consulta si un medicamento esta borrado (para cuando se quiere crear un medicamento)
        /// </summary>
        /// <param name="nombre">nombre del medicamento a buscar</param>
        /// <returns>true si el medicamento existe
        /// false en caso contrario</returns>
        public static bool ConsultarBorrado(string nombre)
        {
            //query de la solicitud
            var query = "SELECT [Nombre] FROM[dbo].[MEDICAMENTO] " +
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