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
    
    public partial class CASAFARMACEUTICA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CASAFARMACEUTICA()
        {
            this.MEDICAMENTOes = new HashSet<MEDICAMENTO>();
        }
    
        public string Nombre { get; set; }
        public string Sede { get; set; }
        public bool Activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICAMENTO> MEDICAMENTOes { get; set; }
    }
}
