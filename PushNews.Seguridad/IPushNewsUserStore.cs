using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace PushNews.Seguridad
{
    public interface IPushNewsUserStore<TUser, TRol, TKey> :
        IQueryableRoleStore<TRol, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>
        where TUser : class, IUser<TKey> 
        where TRol: class, IRole<TKey>
    {
        void AsignarRol(TUser usuario, TKey rolId);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}