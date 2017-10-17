namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo basado en como se recibe la informacion de las recetas
    /// </summary>
    public class RecetaRecibo
    {
        public int numero { get; set; }
        public string imagen { get; set; }
        public string doctor { get; set; }
        public string nombre { get; set; }
        public string medicamentos { get; set; }
    }
}