using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de clientes 
    /// </summary>
    public class RepositorioReceta
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //Consulta todos los clientes de la base de datos
        public static List<Receta> ConsultarRecetas(int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<Receta>();
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
                        List<MedicamentoReceta> productos = ConsultarMedicamentosReceta(reader.GetInt32(1));
                        lista.Add(new Receta
                        {
                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            productos=productos,
                            activo = true
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

        public static bool AgregarReceta(Receta receta, int cliente)
        {
            byte[] imageBytes = Convert.FromBase64String(receta.imagen);

            //Query que consulta la base de datos
            var query = "INSERT INTO [dbo].[RECETA] ([Imagen], [Nombre], [Activo], [Cliente]) " +
                "VALUES(@Imagen, @Nombre, @Activo, @Cliente)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Imagen", imageBytes);
                comando.Parameters.AddWithValue("@Nombre", receta.nombre);
                comando.Parameters.AddWithValue("@Activo", receta.activo);
                comando.Parameters.AddWithValue("@Cliente", cliente);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        public static List<Receta> ConsultarReceta(string nombre, int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<Receta>();
            //Query que consulta la base de datos
            var query = "SELECT [Numero], [Nombre], [Imagen]" +
                " FROM[dbo].[RECETA] WHERE Activo=1 AND Cliente= @cliente AND Nombre= @nombre ";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                comando.Parameters.AddWithValue("@nombre", nombre);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        List<MedicamentoReceta> productos = ConsultarMedicamentosReceta(reader.GetInt32(0));
                        lista.Add(new Receta
                        {
                            nombre = reader.GetString(1),
                            imagen = Convert.ToBase64String((byte[])(reader[2])),
                            productos = productos,
                            activo = true
                        });
                    }
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

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

        public static bool BorrarReceta(string receta, int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[RECETA] SET [ACTIVO]=0" +
                "WHERE Nombre= @nombre AND Cliente =@cliente";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                comando.Parameters.AddWithValue("@nombre", receta);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

    }
}