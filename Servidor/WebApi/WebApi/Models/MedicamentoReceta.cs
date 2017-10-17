namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo de la informacion que se almacena de los medicamentos por receta
    /// </summary>
    public class MedicamentoReceta
    {
        public string medicamento { get; set; }
        public int cantidad { get; set; }
    }
}