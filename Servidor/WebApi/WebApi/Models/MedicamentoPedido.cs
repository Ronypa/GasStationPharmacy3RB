namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo  de la informacion que se almacena de los medicamentos por pedido
    /// </summary>
    public class MedicamentoPedido
    {
        public string medicamento { get; set; }
        public int cantidad { get; set; }
        public int receta { get; set; }
        public int precio { get; set; }
    }
}