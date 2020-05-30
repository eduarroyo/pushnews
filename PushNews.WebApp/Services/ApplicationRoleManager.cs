using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PushNews.Dominio;
using PushNews.Seguridad;
using System.Collections.Generic;
using PushNews.Dominio.Entidades;
using System.Threading.Tasks;

namespace PushNews.WebApp.Services
{
    public class ApplicationRoleManager : RoleManager<Rol, long>
    {
        private IPushNewsRoleProfileStore<Rol, Perfil, long> RpStore
        {
            get
            {
                return (IPushNewsRoleProfileStore<Rol, Perfil, long>)Store;
            }
        }

        public ApplicationRoleManager(IPushNewsRoleProfileStore<Rol, Perfil, long> store)
            : base(store)
        {
        }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            IPushNewsUnitOfWork pushNewsModel = context.Get<IPushNewsUnitOfWork>();
            return new ApplicationRoleManager(new RoleStore(pushNewsModel));
        }

        public IEnumerable<Perfil> Perfiles()
        {
            return RpStore.Perfiles();
        }

        public Perfil Perfil(long perfilID)
        {
            return RpStore.Perfiles().SingleOrDefault(p => p.PerfilID == perfilID);
        }

        public async Task ActualizarPerfil(Perfil perfil)
        {
            await RpStore.GuardarCambiosAsync();
        }

        public async Task NuevoPerfil(Perfil perfil)
        {
            RpStore.NuevoPerfil(perfil);
            await RpStore.GuardarCambiosAsync();
        }

        public async Task EliminarPerfil(Perfil perfil)
        {
            RpStore.EliminarPerfil(perfil);
            await RpStore.GuardarCambiosAsync();
        }

        public async Task EstablecerRoles(Perfil perfil, IEnumerable<long> roles)
        {
            IEnumerable<long> rolesActuales = perfil.Roles != null 
                ? perfil.Roles.Select(p => p.RolID)
                : new long[0];
            long[] rolesAniadir = roles.Where(p => !rolesActuales.Contains(p)).ToArray();
            long[] rolesQuitar = rolesActuales.Where(p => !roles.Contains(p)).ToArray();
            await RpStore.ActualizarRolesAsync(perfil, rolesAniadir, rolesQuitar);
        }

        public Perfil NuevoPerfil()
        {
            return RpStore.NuevoPerfil();
        }
    }
}