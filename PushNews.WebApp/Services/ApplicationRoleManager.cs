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
        private IQueryableRoleStore<Rol, long> RpStore
        {
            get
            {
                return (IQueryableRoleStore<Rol, long>)Store;
            }
        }

        public ApplicationRoleManager(IQueryableRoleStore<Rol, long> store)
            : base(store)
        {
        }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            IPushNewsUnitOfWork pushNewsModel = context.Get<IPushNewsUnitOfWork>();
            return new ApplicationRoleManager(new RoleStore(pushNewsModel));
        }
    }
}