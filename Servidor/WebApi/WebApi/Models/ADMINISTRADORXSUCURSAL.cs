//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ADMINISTRADORXSUCURSAL
    {
        public int Administrador { get; set; }
        public string Sucursal { get; set; }
        public bool Activo { get; set; }
    
        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual SUCURSAL SUCURSAL1 { get; set; }
    }
}