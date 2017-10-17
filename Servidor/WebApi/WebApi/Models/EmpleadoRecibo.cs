namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo del empleado basado en la base de datos contiene 
    /// el modelo de los atributos que se almacenan de los empleados
    /// basado en como se recibe la informacion
    /// </summary>
    public class EmpleadoRecibo
    {
        public int cedula { get; set; }
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string provincia { get; set; }
        public string ciudad { get; set; }
        public string senas { get; set; }
        public string fechaNacimiento { get; set; }
        public string contrasena { get; set; }
        public string sucursal { get; set; }
        public string rol { get; set; }
    }
}