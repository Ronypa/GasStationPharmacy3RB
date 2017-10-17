using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de recetas 
    /// </summary>
    public class RepositorioGeneral
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; " +
            "Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------/
        /// <summary>
        /// Consulta los nombres de las compañias
        /// </summary>
        /// <returns>Lista con los nombres de las compañias</returns>
        public static List<ObjGeneral> ConsultarCompanias()
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre] " +
                " FROM[dbo].[COMPAÑIA] WHERE Activo=1";
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
                        lista.Add(new ObjGeneral
                        {
                            opcion = GetValue<string>(reader["nombre"]),
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
        /// Consulta los nombres de las casas farmaceuticas
        /// </summary>
        /// <returns>Lista con los nombres de las casas farmaceuticas</returns>
        public static List<ObjGeneral> ConsultarCasasFarmaceuticas()
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre] " +
                " FROM[dbo].[CASAFARMACEUTICA] WHERE Activo=1";
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
                        lista.Add(new ObjGeneral
                        {
                            opcion = GetValue<string>(reader["nombre"]),
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
        /// Consulta los nombres de las sucursales de una compañia
        /// </summary>
        /// <returns>Lista con los nombres de las sucursales de una compañia</returns>
        public static List<ObjGeneral> ConsultarSucursales(string compania)
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre] " +
                " FROM[dbo].[SUCURSAL] WHERE Activo=1 AND Compañia = @compania";
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
                    {
                        lista.Add(new ObjGeneral
                        {
                            opcion = GetValue<string>(reader["nombre"]),
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
        /// Consulta los telefonos de un cliente
        /// </summary>
        /// <returns>Lista con los telefonos del cliente</returns>
        public static List<ObjGeneral> ConsultarTelefonos(int cedula)
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Telefono] " +
                " FROM[dbo].[TELEFONOXCLIENTE] WHERE Activo=1 AND Cliente = @cliente";
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
                            opcion = GetValue<string>(reader["telefono"]),
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
        /// Consulta los roles de la empresa
        /// </summary>
        /// <returns>Lista con los nombres de los roles de una compañia</returns>
        public static List<ObjGeneral> ConsultarRoles()
        {
            var lista = new List<ObjGeneral>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre] " +
                " FROM[dbo].[ROL] WHERE Activo=1";
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
                        lista.Add(new ObjGeneral
                        {
                            opcion = GetValue<string>(reader["nombre"]),
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