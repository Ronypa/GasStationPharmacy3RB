using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    public class Receta
    {
        public string nombre { get; set; }
        public int numero { get; set; }
        public bool activo { get; set; }
        public string imagen { get; set; }
        public List<MedicamentoReceta> productos { get; set; }
    }
}