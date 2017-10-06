using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasStationPharmacy.Models
{
    /// <summary>
    /// Modelo del cliente basado en la base de datos contiene 
    /// el modelo de los atributos que se almacenan de los clientes
    /// </summary>
    public class Cliente
    {
        public int cedula { get; set; }
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string provincia { get; set; }
        public string ciudad { get; set; }
        public string señas { get; set; }
        public string fechaNacimiento { get; set; }
        public string contrasena { get; set; }
        public int prioridad { get; set; }
        public bool activo { get; set; }
    }
}