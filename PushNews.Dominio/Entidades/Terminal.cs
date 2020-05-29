using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNews.Dominio.Entidades
{
    public class Terminal
    {
        public Terminal()
        {
            Accesos = new List<ComunicacionAcceso>(0);
        }

        public long TerminalID { get; set; }
        public long AplicacionID { get; set; }
        public string Nombre { get; set; }
        public DateTime UltimaConexionFecha { get; set; }
        public string UltimaConexionIP { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }
        public virtual ICollection<ComunicacionAcceso> Accesos { get; set; }
    }
}
