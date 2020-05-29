using System;

namespace PushNews.Dominio.Entidades
{
    public class Telefono
    {
        public long TelefonoID { get; set; }
        public long AplicacionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Numero { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public virtual Aplicacion Aplicacion { get; set; }
    }
}
