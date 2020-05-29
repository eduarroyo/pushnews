using System.Linq;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PushNews.Security
{
    public interface IPushNewsRoleProfileStore<TRol, TPerfil, TClave>: IQueryableRoleStore<TRol, TClave> where TRol : IRole<TClave>
    {
        IQueryable<TPerfil> Perfiles();

        TPerfil Perfil(TClave perfilID);

        void NuevoPerfil(TPerfil perfil);

        TPerfil NuevoPerfil();

        void EliminarPerfil(TPerfil perfil);

        void RenombrarPerfil(TPerfil perfil, string nuevoNombre);

        void AniadirRol(TPerfil perfil, TClave rolID);

        void QuitarRol(TPerfil perfil, TClave rolID);

        void AniadirRoles(TPerfil perfil, IEnumerable<TClave> roles);

        void QuitarRoles(TPerfil perfil, IEnumerable<TClave> roles);

        Task GuardarCambiosAsync();

        Task ActualizarRolesAsync(TPerfil perfil, IEnumerable<long> rolesAniadir, IEnumerable<long> rolesQuitar);
        void ActualizarRoles(TPerfil perfil, IEnumerable<long> rolesAniadir, IEnumerable<long> rolesQuitar);
    }
}