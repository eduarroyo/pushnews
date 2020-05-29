using System;

namespace PushNews.Dominio.Entidades
{
    public class ComunicacionAcceso
    {
        public long ComunicacionAccesoID { get; set; }
        public long ComunicacionID { get; set; }
        public long TerminalID { get; set; }
        public DateTime Fecha { get; set; }

        public long? AsociadoID { get; set; }

        public virtual Comunicacion Comunicacion { get; set; }
        public virtual Terminal Terminal { get; set; }
        public virtual Asociado Asociado { get; set; }
    }
}
