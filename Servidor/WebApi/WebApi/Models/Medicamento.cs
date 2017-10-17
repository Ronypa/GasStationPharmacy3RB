namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo de medicamento basado en los atributos de la base de datos
    /// </summary>
    public class Medicamento
    {
        public string nombre { get; set; }
        public bool prescripcion { get; set; }
        public int cantidad { get; set; }
        public string casaFarmaceutica { get; set; }
        public int precio { get; set; }
    }
}