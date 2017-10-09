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
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //Consulta todos los clientes de la base de datos
        public static List<Medicamento> ConsultarMedicamento()
        {
            //Lista que almacena los clientes
            var lista = new List<Medicamento>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Precio], [Prescripcion], [Cantidad]" +
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
                            nombre = reader.GetString(0),
                            precio = reader.GetInt32(1),
                            prescripcion = reader.GetBoolean(2),
                            cantidad = reader.GetInt32(3),
                            activo = true
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }         
        }

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

    }
}