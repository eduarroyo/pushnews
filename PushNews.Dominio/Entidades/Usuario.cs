using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PushNews.Dominio.Entidades
{
    public class Usuario: IUser<long>
    {
        public Usuario()
        {
            Aplicaciones = new List<Aplicacion>(0);
            Comunicaciones = new List<Comunicacion>(0);
            Categorias = new List<Categoria>(0);
        }
        
        public long UsuarioID { get; set; }
        public long RolID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Movil { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string MarcaSeguridad { get; set; }
        public bool Activo { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool MovilConfirmado { get; set; }
        public int AccesosFallidos { get; set; }
        public bool Externo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public DateTime? FinalBloqueoUtc { get; set; }
        public bool DosFactoresHabilitado { get; set; }
        public long? ProveedorID { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public string Locale { get; set; }

        public virtual Rol Rol { get; set; }
        public virtual ICollection<Aplicacion> Aplicaciones { get; set; }
        public virtual ICollection<Comunicacion> Comunicaciones { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }

        /// <summary>
        /// Obligado por la interfaz IUser. Esta propiedad es un envoltorio de UsuarioID.
        /// </summary>
        public long Id
        {
            get { return UsuarioID; }
            set { UsuarioID = value; }
        }

        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Usuario, long> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public ClaimsIdentity GenerateUserIdentity(UserManager<Usuario, long> manager)
        {
            return manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public string ApellidosNombre
        {
            get
            {
                return $"{Apellidos}, {Nombre}";
            }
        }
    }
}