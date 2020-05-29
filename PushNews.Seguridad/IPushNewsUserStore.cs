using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace PushNews.Security
{
    public interface IPushNewsUserStore<TUser, TPerfil, TKey> :
        IUserRoleStore<TUser, TKey>,
        IUserClaimStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>,
        IUserLoginStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>
        where TUser : class, IUser<TKey>
    {
        void ActualizarPerfiles(TUser usuario, IEnumerable<TKey> perfilesAniadir, IEnumerable<TKey> perfilesQuitar);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}