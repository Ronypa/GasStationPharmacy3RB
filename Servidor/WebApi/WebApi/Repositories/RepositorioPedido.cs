using System;
using System.Collections.Generic;
using GasStationPharmacy.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Data;

namespace GasStationPharmacy.Repositories
{
    /// <summary>
    /// Clase que almacena las acciones relacionadas con la base de datos para la gestion de pedidos
    /// </summary>
    public class RepositorioPedido
    {
        //Conexion con la base de datos
        static String conexionString = "Data Source = (local)\\SQLEXPRESS;" +
            " Initial Catalog =GasStationPharmacy;Integrated Security=True";
        static SqlConnection conexion = new SqlConnection(conexionString);

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta toda la informacion de un pedido
        /// </summary>
        /// <param name="cedula">cliente que solicita la informacion</param>
        /// <param name="numero">pedido a consultar</param>
        /// <returns></returns>
        public static List<PedidoEnvio> ConsultarDetallePedido(int numero)
        {
            //Lista que almacena la infomacion del pedido
            var lista = new List<PedidoEnvio>();
            //Query que consulta la base de datos
            var query = "SELECT [COMPAÑIA].Nombre AS compNomb, [PEDIDO].Nombre  AS pedNomb, [PEDIDO].Numero, " +
                "[PEDIDO].Estado, [PEDIDO].FechaRecojo, [PEDIDO].Telefono, [PEDIDO].Sucursal " +
                "FROM[dbo].[PEDIDO] INNER JOIN [SUCURSAL] " +
                "ON PEDIDO.Sucursal = SUCURSAL.Nombre " +
                "INNER JOIN [COMPAÑIA] " +
                "ON SUCURSAL.Compañia = COMPAÑIA.Nombre " +
                "WHERE [PEDIDO].Activo=1 AND [PEDIDO].Numero = @numero";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(2));
                        lista.Add(new PedidoEnvio
                        {
                            compania = GetValue<string>(reader["compNomb"]),
                            nombre = GetValue<string>(reader["pedNomb"]),
                            numero = GetValue<int>(reader["numero"]),
                            estado = GetValue<char>(reader["estado"]),
                            fecha_recojo = GetValue<string>(reader["fechaRecojo"]),
                            telefono = GetValue<string>(reader["telefono"]),
                            sucursal = GetValue<string>(reader["sucursal"]),
                            productos = productos
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
        /// Consulta el nombre y numero de todos los pedidos de un cliente 
        /// </summary>
        /// <param name="cedula">cliente que consulta los pedidos</param>
        /// <returns>lista con los pedidos de un cliente</returns>
        public static List<ObjGeneral2> ConsultarPedidos(int cedula)
        {
            //Lista que almacena los pedidos
            var lista = new List<ObjGeneral2>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado] " +
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
                        lista.Add(new ObjGeneral2
                        {
                            opcion = GetValue<string>(reader["nombre"]),
                            opcion3 = GetValue<string>(reader["estado"]),
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
        /// Consulta todos los pedidos segun el tipo p:preparados, f:facturados, n:nuevos, r:retirados
        /// </summary>
        /// <returns>lista con la informacion de todos los pedidos solicitados</returns>
        public static List<PedidoEnvio> ConsultarPedidosPorEstado(char estado, string sucursal)
        {
            //Lista que almacena los clientes
            var lista = new List<PedidoEnvio>();
            //Query que consulta la base de datos
            var query = "SELECT [Nombre], [Numero], [Estado], [FechaRecojo], [Telefono], [Sucursal], [Cliente]" +
                " FROM[dbo].[PEDIDO] WHERE Activo=1 AND Estado=@estado AND Sucursal=@sucursal";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@estado", estado);
                comando.Parameters.AddWithValue("@sucursal", sucursal);
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())//Se agrega la informacion leida a la lista
                    {
                        List<MedicamentoPedido> productos = ConsultarMedicamentosPedido(reader.GetInt32(1));
                        lista.Add(new PedidoEnvio{
                            nombre = GetValue<string>(reader["nombre"]),
                            numero = GetValue<int>(reader["numero"]),
                            estado = GetValue<char>(reader["estado"]),
                            fecha_recojo = GetValue<string>(reader["fechaRecojo"]),
                            telefono = GetValue<string>(reader["telefono"]),
                            sucursal = GetValue<string>(reader["sucursal"]),
                            cliente = GetValue<int>(reader["cliente"]),
                            productos = productos
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
        /// Cambia el estado de un pedido
        /// </summary>
        /// <param name="numero">pedido a cambiar su estado</param>
        /// <param name="estado">estado a establecer al pedido</param>
        /// <returns>true si se cambio correctamente/false en caso contrario</returns>
        public static bool CambiarEstadoPedido(int numero,char estado)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [Estado]=@estado " +
                "WHERE Numero= @numero";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.Parameters.AddWithValue("@estado", estado);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { conexion.Close(); return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta los medicamentos de un pedido
        /// </summary>
        /// <param name="pedido">pedido a consultar</param>
        /// <returns>lista de medicamentos</returns>
        public static List<MedicamentoPedido> ConsultarMedicamentosPedido(int pedido)
        {
            String conexionString = "Data Source = (local)\\SQLEXPRESS; Initial Catalog =GasStationPharmacy;" +
            "Integrated Security=True";
            SqlConnection conexion = new SqlConnection(conexionString);
            
            //Lista que almacena los clientes
            var lista = new List<MedicamentoPedido>();
            //Query que consulta la base de datos
            var query = "SELECT [Medicamento], [MEDICAMENTOXPEDIDO].Cantidad, [Precio], [RECETA]" +
                " FROM[dbo].[MEDICAMENTOXPEDIDO] INNER JOIN [MEDICAMENTO] " +
                "ON MEDICAMENTOXPEDIDO.Medicamento =MEDICAMENTO.Nombre " +
                "WHERE MEDICAMENTOXPEDIDO.Activo=1 AND Pedido= @pedido";
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
                            medicamento = GetValue<string>(reader["medicamento"]),
                            cantidad = GetValue<int>(reader["cantidad"]),
                            precio = GetValue<int>(reader["precio"]),
                            receta = GetValue<int>(reader["receta"])
                        });
                }
                conexion.Close();
                return lista;
            }
            catch (Exception) { return null; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borrado logico de un pedido
        /// </summary>
        /// <param name="pedido">pedido a borrar</param>
        /// <param name="cedula">cliente que lo solicita</param>
        /// <returns>true si lo logra borrar / false en caso contrario</returns>
        public static bool BorrarPedido(int pedido)
        {
            //query de la solicitud
            var query = "UPDATE [dbo].[PEDIDO] SET [ACTIVO]=0" +
                "WHERE Numero= @numero";
            //ejecuta el query
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", pedido);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Crea un pedido nuevo
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public static bool AgregarPedido(PedidoRecibo pedido)
        {
            //Query que consulta la base de datos
            var query = "INSERT INTO[dbo].[PEDIDO] ([Estado], [FechaRecojo]," +
                "[Telefono], [Sucursal], [Cliente], [Activo], [Nombre]) " +
                "OUTPUT inserted.Numero " +
                "VALUES(@Estado, @FechaRecojo, @Telefono, @Sucursal, @Cliente, " +
                "@Activo, @Nombre)";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Estado", 'n');
                comando.Parameters.AddWithValue("@FechaRecojo", pedido.fecha_recojo);
                comando.Parameters.AddWithValue("@Telefono", pedido.telefono);
                comando.Parameters.AddWithValue("@Sucursal", pedido.sucursal);
                comando.Parameters.AddWithValue("@Cliente", pedido.cliente);
                comando.Parameters.AddWithValue("@Activo", true);
                comando.Parameters.AddWithValue("@Nombre", pedido.nombre);
                int modified = (int)comando.ExecuteScalar();
                pedido.numero = modified;
                comando.Dispose();
                conexion.Close();
                AgregarMedicamentosPedido(pedido);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega los medicamentos de un pedido
        /// </summary>
        /// <param name="pedido">informacion del pedido</param>
        /// <returns>true si agrega los telefonos / false en caso contrario</returns>
        public static bool AgregarMedicamentosPedido(PedidoRecibo pedido)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            MedicamentoPedido[] medicamentos = js.Deserialize<MedicamentoPedido[]>(pedido.productos);
            foreach (MedicamentoPedido medicamento in medicamentos) 
            {
                //Query que consulta la base de datos
                var query = "INSERT INTO [dbo].[MEDICAMENTOXPEDIDO] ([Pedido] ,[Medicamento], " +
                    "[Activo], [Receta],[Cantidad]) " +
                    " VALUES(@Pedido, @Medicamento, @Activo, @Receta,@Cantidad)";
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Pedido", pedido.numero);
                    if (medicamento.receta == 0)
                    {comando.Parameters.AddWithValue("@Receta", DBNull.Value);}
                    else {comando.Parameters.AddWithValue("@Receta", medicamento.receta);}
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
        /// Actualiza la informacion de un pedido
        /// </summary>
        /// <param name="pedido">informacion a actualizar del pedido</param>
        /// <returns>true si se actualizo / false en caso contrario</returns>
        public static bool ActualizarPedido(PedidoRecibo pedido)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[PEDIDO] SET [Nombre] = @nombre, " +
                "[FechaRecojo]=@fecha, [Telefono]=@telefono, [Sucursal]=@sucursal " +
                "WHERE Numero=@numero";
            //Se ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", pedido.nombre);
                comando.Parameters.AddWithValue("@fecha", pedido.fecha_recojo);
                comando.Parameters.AddWithValue("@telefono", pedido.telefono);
                comando.Parameters.AddWithValue("@sucursal", pedido.sucursal);
                comando.Parameters.AddWithValue("@numero", pedido.numero);
                comando.ExecuteNonQuery();
                comando.Dispose();
                conexion.Close();
                ActualizarCantidadesPedido(pedido);
                return true;
            }
            catch (Exception) { return false; }
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta si un pedido aun no se ha preparado para poder actualizarlo
        /// </summary>
        /// <param name="numero">numero de pedido a crear</param>
        /// <returns>true si el pedido se puede modificar
        /// false en caso contrario</returns>
        public static bool ConsultarParaActualizar(int numero, string estado)
        {
            //query de la solicitud
            var query = "SELECT [Nombre] FROM[dbo].[PEDIDO] " +
                "WHERE Numero= @numero AND Estado = @estado";
            //ejecuta el query
            try
            {
                conexion.Close();
                conexion.Open();
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.Parameters.AddWithValue("@estado", estado);
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
        /// Actualiza las cantidades de los medicamentos de un pedido
        /// </summary>
        /// <param name="pedido">informacion a actualizar del pedido</param>
        /// <returns>true si se actualizo / false en caso contrario</returns>
        public static bool ActualizarCantidadesPedido(PedidoRecibo pedido)
        {
            //Query que consulta la base de datos
            var query = "UPDATE [dbo].[MEDICAMENTOXPEDIDO] SET [Cantidad] = @cantidad " +
            "WHERE Pedido=@pedido AND Medicamento=@medicamento";

            JavaScriptSerializer js = new JavaScriptSerializer();
            MedicamentoPedido[] medicamentos = js.Deserialize<MedicamentoPedido[]>(pedido.productos);
            foreach (MedicamentoPedido medicamento in medicamentos)//Por cada telefono 
            {
                //Se ejecuta el query
                try
                {
                    conexion.Close();
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@cantidad", medicamento.cantidad);
                    comando.Parameters.AddWithValue("@pedido", pedido.numero);
                    comando.Parameters.AddWithValue("@medicamento", medicamento.medicamento);
                    comando.ExecuteNonQuery();
                    comando.Dispose();
                    conexion.Close();
                    return true;
                }
                catch (Exception) { return false; }
            }
            return true;
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