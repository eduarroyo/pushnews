using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class AplicacionCaracteristica
    {
        public AplicacionCaracteristica()
        {
            Aplicaciones = new List<Aplicacion>(0);
        }

        public long AplicacionCaracteristicaID { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<Aplicacion> Aplicaciones { get; set; }
    }
}