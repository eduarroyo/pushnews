//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushNews.PublicadorPush
{
    using System;
    using System.Collections.Generic;
    
    public partial class Aplicacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aplicacion()
        {
            this.Categorias = new HashSet<Categoria>();
            this.Parametros = new HashSet<Parametro>();
        }
    
        public long AplicacionID { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }
        public bool Activo { get; set; }
        public string CloudKey { get; set; }
        public string SubDominio { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public Nullable<long> LogotipoID { get; set; }
        public string ApiKey { get; set; }
        public string PlayStoreUrl { get; set; }
        public string AppStoreUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categoria> Categorias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parametro> Parametros { get; set; }
    }
}
