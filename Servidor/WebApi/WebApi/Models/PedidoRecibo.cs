namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo de informacion para la gestion de pedidos basado en como se recibe la informacion
    /// </summary>
    public class PedidoRecibo
    {
        public string nombre { get; set; }
        public int numero { get; set; }
        public string fecha_recojo { get; set; }
        public string telefono { get; set; }
        public string sucursal { get; set; }
        public int cliente { get; set; }
        public char estado { get; set; }
        public string productos { get; set; }
    }
}