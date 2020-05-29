using System;

namespace PushNews.Dominio.Entidades
{
    public class Localizacion
    {
        public long LocalizacionID { get; set; }
        public long AplicacionID { get; set; }
        public DateTime Fecha { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
    }
}
