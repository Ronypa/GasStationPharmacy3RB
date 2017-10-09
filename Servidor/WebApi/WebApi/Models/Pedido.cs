using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    public class Pedido
    {
        public string nombre { get; set; }
        public int numero { get; set; }
        public string fecha_recojo { get; set; }
        public int telefono { get; set; }
        public string sucursal { get; set; }
        public int cliente { get; set; }
        public int estado { get; set; }
        public bool activo { get; set; }
        public List<MedicamentoPedido> productos { get; set; }
    }
}