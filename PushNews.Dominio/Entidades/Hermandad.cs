using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class Hermandad
    {
        public Hermandad()
        {
            Activo = true;
            Nombre = "";
            IglesiaNombre = "";
            IglesiaDireccion = "";
            Tags = "";
        }

        public long HermandadID { get; set; }
        public long AplicacionID { get; set; }
        public string Nombre { get; set; }
        public long? LogotipoDocumentoID { get; set; }
        public string IglesiaNombre { get; set; }
        public string IglesiaDireccion { get; set; }
        public double? IglesiaLatitud { get; set; }
        public double? IglesiaLongitud { get; set; }
        public bool Activo { get; set; }

        /// <summary>
        /// Contiene tags descriptivos de la hermandad separados por comas, para propósitos de 
        /// clasificación y búsqueda.
        /// </summary>
        public string Tags { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
        public virtual Documento Logotipo { get; set; }
        public virtual ICollection<Ruta> Rutas { get; set; }
    }
}