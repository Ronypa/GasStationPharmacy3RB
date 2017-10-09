using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    public class MedicamentoPedido
    {
        public string medicamento { get; set; }
        public int precio { get; set; }
        public int cantidad { get; set; }
    }
}