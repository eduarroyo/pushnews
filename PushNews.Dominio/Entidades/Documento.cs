using PushNews.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class Documento
    {
        public Documento()
        {
            ComunicacionesAdjunto = new List<Comunicacion>(0);
            ComunicacionesImagen = new List<Comunicacion>(0);
        }

        public long DocumentoID { get; set; }
        public long AplicacionID { get; set; }
        public DocumentoTipo Tipo { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Mime { get; set; }
        public long Tamano { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
        public virtual ICollection<Comunicacion> ComunicacionesAdjunto { get; private set; }
        public virtual ICollection<Comunicacion> ComunicacionesImagen { get; private set; }
    }
}
