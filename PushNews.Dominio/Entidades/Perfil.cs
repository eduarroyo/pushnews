using System.Collections.Generic;

namespace PushNews.Dominio.Entidades
{
    public class Perfil
    {
        public Perfil()
        {
            Roles = new List<Rol>(0);
            Usuarios = new List<Usuario>(0);
        }

        public long PerfilID { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<Rol> Roles { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}