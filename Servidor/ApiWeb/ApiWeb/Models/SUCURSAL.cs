//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SUCURSAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUCURSAL()
        {
            this.ADMINISTRADORXSUCURSALs = new HashSet<ADMINISTRADORXSUCURSAL>();
            this.EMPLEADOes = new HashSet<EMPLEADO>();
            this.PEDIDOes = new HashSet<PEDIDO>();
        }
    
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public string Cuidad { get; set; }
        public string Señas { get; set; }
        public string Descripcion { get; set; }
        public string Compañia { get; set; }
        public bool Activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADMINISTRADORXSUCURSAL> ADMINISTRADORXSUCURSALs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADOes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDOes { get; set; }
        public virtual COMPAÑIA COMPAÑIA1 { get; set; }
    }
}
