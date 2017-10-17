using System;
using System.Collections.Generic;

namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo del cliente basado en la base de datos contiene 
    /// el modelo de los atributos que se almacenan de los clientes basado en 
    /// caracteristicas de envio
    /// </summary>
    public class ClienteEnvio
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
        public int prioridad { get; set; }
        public List<String> telefonos { get; set; }
        public List<Padecimiento> padecimientos { get; set; }
    }
}