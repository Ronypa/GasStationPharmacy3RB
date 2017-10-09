using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la tabla de clientes 
    /// </summary>
    public class RepositorioPedido
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //Consulta todos los clientes de la base de datos
        public static List<Pedido> ConsultarPedidos(int cedula)
        {
            //Lista que almacena los clientes
            var lista = new List<Pedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Cliente= @cliente";
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
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new Pedido
                        {
                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            estado = reader.GetInt32(2),
                            fecha_recojo = reader.GetDateTime(3).ToString(),
                            telefono = reader.GetInt32(4),
                            sucursal = reader.GetString(5),
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


        public static List<Pedido> ConsultarPedidosNuevos()
        {
            //Lista que almacena los clientes
            var lista = new List<Pedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal], [Cliente]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Estado=1";
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
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new Pedido{
                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            estado = reader.GetInt32(2),
                            fecha_recojo = reader.GetDateTime(3).ToString(),
                            telefono = reader.GetInt32(4),
                            sucursal = reader.GetString(5),
                            cliente = reader.GetInt32(6),
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

        public static List<Pedido> ConsultarPedidosPreparados()
        {
            //Lista que almacena los clientes
            var lista = new List<Pedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal], [Cliente]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Estado=2";
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
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new Pedido
                        {
                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            estado = reader.GetInt32(2),
                            fecha_recojo = reader.GetDateTime(3).ToString(),
                            telefono = reader.GetInt32(4),
                            sucursal = reader.GetString(5),
                            cliente = reader.GetInt32(6),
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

        public static List<Pedido> ConsultarPedidosFacturados()
        {
            //Lista que almacena los clientes
            var lista = new List<Pedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal], [Cliente]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Estado=3";
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
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new Pedido
                        {
                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            estado = reader.GetInt32(2),
                            fecha_recojo = reader.GetDateTime(3).ToString(),
                            telefono = reader.GetInt32(4),
                            sucursal = reader.GetString(5),
                            cliente = reader.GetInt32(6),
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

        public static List<Pedido> ConsultarPedidosRetirados()
        {
            //Lista que almacena los clientes
            var lista = new List<Pedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal], [Cliente]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Estado=4";
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
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new Pedido
                        {

                            nombre = reader.GetString(0),
                            numero = reader.GetInt32(1),
                            estado = reader.GetInt32(2),
                            fecha_recojo = reader.GetDateTime(3).ToString(),
                            telefono = reader.GetInt32(4),
                            sucursal = reader.GetString(5),
                            cliente = reader.GetInt32(6),
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

        public static bool PedidoPreparado(int numero)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [Estado]=2" +
                "WHERE Numero= @numero";
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

        public static bool PedidoFacturado(int numero)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [Estado]=3" +
                "WHERE Numero= @numero";
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

        public static bool PedidoRetirado(int numero)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [Estado]=4" +
                "WHERE Numero= @numero";
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

        public static List<MedicamentoPedido> ConsultarMedicamentosPedido(int pedido)
        {
            String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;Integrated Security=True";
            SqlConnection conexion = new SqlConnection(conexionString);
            //Lista que almacena los clientes
            var lista = new List<MedicamentoPedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Medicamento], [MEDICAMENTOXPEDIDO].Cantidad, [Precio]" +
                " FROM[dbo].[MEDICAMENTOXPEDIDO],[MEDICAMENTO] WHERE MEDICAMENTOXPEDIDO.Activo=1 AND Pedido= @pedido AND" +
                " MEDICAMENTOXPEDIDO.Medicamento =MEDICAMENTO.Nombre ";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@pedido", pedido);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                        lista.Add(new MedicamentoPedido
                        {
                            medicamento = reader.GetString(0),
                            cantidad = reader.GetInt32(1),
                            precio = reader.GetInt32(2)
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        public static bool BorrarPedido(string pedido, int cedula)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [ACTIVO]=0" +
                "WHERE Nombre= @nombre AND Cliente =@cliente";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@cliente", cedula);
                comando.Parameters.AddWithValue("@nombre", pedido);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

    }
}