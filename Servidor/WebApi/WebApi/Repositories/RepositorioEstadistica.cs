using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de recetas 
    /// </summary>
    public class RepositorioEstadistica
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta los 5 productos mas vendidos por la compañia
        /// </summary>
        /// <param name="compania">compañia a consultar la estadistica</param>
        /// <returns>lista con el producto y la cantidad</returns>
        public static List<ObjGeneralEstadistica> ConsultarProductosCompania(string compania)
        {
            var lista = new List<ObjGeneralEstadistica>();
            //Query que consulta la base de datos
            var query = "SELECT top 5 MEDICAMENTOXPEDIDO.Medicamento AS producto, " +
                "SUM(MEDICAMENTOXPEDIDO.Cantidad) as cantidad FROM MEDICAMENTOXPEDIDO " +
                "INNER JOIN PEDIDO ON MEDICAMENTOXPEDIDO.Pedido = PEDIDO.Numero " +
                "INNER JOIN SUCURSAL ON PEDIDO.Sucursal = SUCURSAL.Nombre " +
                "Where SUCURSAL.Compañia = @Compania " +
                "Group by Medicamento " +
                "order by cantidad desc";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Compania", compania);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        lista.Add(new ObjGeneralEstadistica
                        {
                            opcion = GetValue<string>(reader["producto"]),
                            cantidad = GetValue<int>(reader["cantidad"])
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta los 5 productos mas vendidos en total por las compañias
        /// </summary>
        /// <returns>lista con el producto y la cantidad de los 5 mas vendidos</returns>
        public static List<ObjGeneralEstadistica> ConsultarProductosTotal()
        {
            var lista = new List<ObjGeneralEstadistica>();
            //Query que consulta la base de datos
            var query = "SELECT top 5 Medicamento as producto, SUM(Cantidad) as cantidad " +
                "FROM MEDICAMENTOXPEDIDO " +
                "Group by Medicamento " +
                "order by cantidad desc";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        lista.Add(new ObjGeneralEstadistica
                        {
                            opcion = GetValue<string>(reader["producto"]),
                            cantidad = GetValue<int>(reader["cantidad"])
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Obtiene el numero total de ventas de la compañia
        /// </summary>
        /// <param name="compania">compañia a consultar</param>
        /// <returns>cantidad de ventas de la compañia</returns>
        public static int VentasTotales (string compania)
        {
            //Query que consulta la base de datos
            var query = "SELECT top 5 COUNT(1) as cantidad FROM PEDIDO " +
                "INNER JOIN SUCURSAL ON PEDIDO.Sucursal = SUCURSAL.Nombre " +
                "Where SUCURSAL.Compañia = @Compania";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Compania", compania);
                var reader = comando.ExecuteReader();
                reader.Read();
                var cantidad = GetValue<int>(reader["cantidad"]);
                conexion.Close();
                return cantidad;
            }
            catch (Exception) { return -1; }
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