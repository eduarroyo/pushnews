using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace PushNews.Seguridad
{
    public interface IPushNewsAsociadoStore<TUser, TKey> :
        IUserPasswordStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>
        where TUser : class, IUser<TKey>
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        Task<TUser> FindByIdAsync(TKey asociadoID, long aplicacionID);
        Task<TUser> FindByNameAsync(string codigo, long aplicacionID);
        Task<TUser> FindUserAsync(string codigo, string subdominio, string apikey);
    }
}