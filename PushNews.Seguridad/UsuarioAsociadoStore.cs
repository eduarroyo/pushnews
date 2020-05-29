using System.Linq;
using System.Threading.Tasks;
using System;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;

namespace PushNews.Security
{
    public class AsociadoStore :
        IPushNewsAsociadoStore<Asociado, long>
    {
        private IPushNewsUnitOfWork context;
        private readonly bool isDisposable;

        public IQueryable<Asociado> Users
        {
            get { return context.Asociados.Where(u => !u.Eliminado); }
        }

        public AsociadoStore(IPushNewsUnitOfWork context)
        {
            this.context = context;
            isDisposable = false;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }


        #region IUserPasswordStore

        public Task SetPasswordHashAsync(Asociado user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Clave = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(Asociado user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Clave);
        }

        public Task<bool> HasPasswordAsync(Asociado user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            bool hasPassword = !string.IsNullOrEmpty(user.Clave);

            return Task.FromResult(hasPassword);
        }

        #endregion
        

        #region IUserStore

        public void Dispose()
        {
            if (context == null)
            {
                return;
            }

            if (isDisposable)
            {
                ((IDisposable)context).Dispose();
            }

            context = null;
        }

        public Task CreateAsync(Asociado user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (context.Asociados.Any(u => !u.Eliminado && u.Codigo == user.Codigo && user.AplicacionID == u.AplicacionID))
            {
                throw new Exception("Otro asociado está utilizando el código.");
            }
            user.Activo = true;
            context.Asociados.Add(user);
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(Asociado user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if(context.Asociados.Any(u => u.AsociadoID != user.AsociadoID
                                       && !u.Eliminado && u.Codigo == user.Codigo
                                       && u.AplicacionID == user.AplicacionID))
            {
                throw new Exception("Otro asociado está utilizando el código.");
            }

            Asociado userModificar = context.Asociados.Single(u => u.AsociadoID == user.AsociadoID
                                                                && u.AplicacionID == user.AplicacionID);
            return context.SaveChangesAsync();
        } 

        public async Task DeleteAsync(Asociado user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Eliminado = true;
            await context.SaveChangesAsync();
        }

        public Task<Asociado> FindByIdAsync(long asociadoID)
        {
            Asociado user = context.Asociados.FirstOrDefault(u => u.AsociadoID == asociadoID
                                                               && !u.Eliminado);
            return Task.FromResult(user);
        }

        public Task<Asociado> FindByIdAsync(long asociadoID, long aplicacionID)
        {
            Asociado user = context.Asociados.FirstOrDefault(u => u.AsociadoID == asociadoID
                                                               && u.AplicacionID == aplicacionID
                                                               && !u.Eliminado);
            return Task.FromResult(user);
        }

        public Task<Asociado> FindByNameAsync(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                throw new ArgumentNullException("codigo");
            }
            Asociado user = context.Asociados.FirstOrDefault(u => u.Codigo == codigo && !u.Eliminado);
            return Task.FromResult(user);
        }

        public Task<Asociado> FindByNameAsync(string codigo, long aplicacionID)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                throw new ArgumentNullException("codigo");
            }
            Asociado user = context.Asociados
                .FirstOrDefault(u => u.Codigo == codigo && !u.Eliminado && u.AplicacionID == aplicacionID);
            return Task.FromResult(user);
        }

        public Task<Asociado> FindUserAsync(string codigo, string subdominio, string apikey)
        {
            Asociado user = context.Asociados.FirstOrDefault(u => u.Codigo == codigo 
                                                               && !u.Eliminado
                                                               && u.Aplicacion.SubDominio == subdominio
                                                               && u.Aplicacion.ApiKey == apikey
                                                               && u.Aplicacion.Caracteristicas.Any(c => c.Nombre == "Asociados"));
            return Task.FromResult(user);
        }

        #endregion
    }
}
