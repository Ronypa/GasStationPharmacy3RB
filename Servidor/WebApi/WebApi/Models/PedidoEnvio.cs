using System.Collections.Generic;

namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo de informacion para la gestion de pedidos
    /// </summary>
    public class PedidoEnvio
    {
        public string nombre { get; set; }
        public int numero { get; set; }
        public string fecha_recojo { get; set; }
        public string telefono { get; set; }
        public string sucursal { get; set; }
        public string compania { get; set; }
        public int cliente { get; set; }
        public char estado { get; set; }
        public List<MedicamentoPedido> productos { get; set; }
    }
}