using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    public class Medicamento
    {
        public string nombre { get; set; }
        public bool prescripcion { get; set; }
        public int cantidad { get; set; }
        public string casaFarmaceutica { get; set; }
        public bool activo { get; set; }
        public int precio { get; set; }
    }
}