using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PushNews.Dominio.Entidades
{
    public class Asociado: IUser<long>
    {
        public Asociado()
        {
            ComunicacionesAccesos = new List<ComunicacionAcceso>(0);
        }

        public long AsociadoID { get; set; }
        public long AplicacionID { get; set; }
        public string Codigo { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string  CodigoPostal { get; set; }
        public string Provincia { get; set; }

        public string Telefono { get; set; }
        public string Email { get; set; }

        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }

        public virtual Aplicacion Aplicacion { get; set; }

        public virtual ICollection<ComunicacionAcceso> ComunicacionesAccesos { get; set; }

        public long Id
        {
            get { return AsociadoID; }
        }

        public string UserName
        {
            get { return Codigo; }
            set { Codigo = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Asociado, long> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public ClaimsIdentity GenerateUserIdentity(UserManager<Asociado, long> manager)
        {
            return manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}