#region License and copyright notice
/* 
 * ASP.NET Identity provider for Telerik Data Access
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using Claim = System.Security.Claims.Claim;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace PushNews.Security
{
    public class UserStore :
        IPushNewsUserStore<Usuario, Perfil, long>
    {
        private IPushNewsUnitOfWork context;
        private readonly bool isDisposable;

        public UserStore(IPushNewsUnitOfWork context)
        {
            this.context = context;
            isDisposable = false;
        }

        #region IUsuariosClaimstore
            
        public void ActualizarPerfiles(Usuario usuario, IEnumerable<long> perfilesAniadir, IEnumerable<long> perfilesQuitar)
        {
            Usuario usr = context.Usuarios.Single(e => e.UsuarioID == usuario.UsuarioID);

            if (perfilesQuitar != null && perfilesQuitar.Any())
            {
                foreach (var pq in perfilesQuitar)
                {
                    Perfil pQuitar = usr.Perfiles.SingleOrDefault(p => p.PerfilID == pq);
                    if (pQuitar != null)
                    {
                        usr.Perfiles.Remove(pQuitar);
                    }
                }
            }
            if (perfilesAniadir != null && perfilesAniadir.Any())
            {
                IQueryable<Perfil> pAniadir = context.Perfiles.Where(p => perfilesAniadir.Contains(p.PerfilID));
                foreach (var p in pAniadir)
                {
                    usr.Perfiles.Add(p);
                }
            }
            //context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<IList<Claim>> GetClaimsAsync(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<PushNews.Dominio.Entidades.Claim> eclaims = await context.Claims
                .Where(c => c.UsuarioID == user.Id)
                .ToListAsync();

            var sclaims = new List<Claim>();
            foreach(var cl in eclaims)
            {
                sclaims.Add(new Claim(cl.Tipo, cl.Valor));
            }

            return sclaims;
        }

        public Task<int> AddClaimAsync(Usuario user, Claim claim) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (claim == null) {
                throw new ArgumentNullException("claim");
            }

            bool alreadyHasClaim = GetUserClaim(user, claim) != null;

            if (!alreadyHasClaim) {
                context.Claims.Add(new PushNews.Dominio.Entidades.Claim(user.Id, claim));
            }
            return context.SaveChangesAsync();
        }

        public Task RemoveClaimAsync(Usuario user, Claim claim) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (claim == null) {
                throw new ArgumentNullException("claim");
            }

            PushNews.Dominio.Entidades.Claim userClaim = GetUserClaim(user, claim);
            context.Claims.Remove(userClaim);
            return context.SaveChangesAsync();
        }

        #endregion


        #region IUserEmailStore

        public Task SetEmailAsync(Usuario user, string email) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.EmailConfirmado);
        }

        public Task SetEmailConfirmedAsync(Usuario user, bool confirmed) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmado = confirmed;

            return Task.FromResult(0);
        }

        public Task<Usuario> FindByEmailAsync(string email) {
            if (email == null) {
                throw new ArgumentNullException("email");
            }

            Usuario user = context.Usuarios.FirstOrDefault(u => u.Email == email);

            return Task.FromResult(user);
        }

        #endregion


        #region IUserLockoutStore

        public Task<DateTimeOffset> GetLockoutEndDateAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if(user.FinalBloqueoUtc.HasValue){
                return Task.FromResult(new DateTimeOffset(DateTime.SpecifyKind(user.FinalBloqueoUtc.Value, DateTimeKind.Utc)));
            }

            return Task.FromResult(new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(Usuario user, DateTimeOffset lockoutEnd) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (lockoutEnd == DateTimeOffset.MinValue) {
                user.FinalBloqueoUtc = null;
            }
            else {
                user.FinalBloqueoUtc = lockoutEnd.UtcDateTime;
            }

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.AccesosFallidos++;

            return Task.FromResult(user.AccesosFallidos);
        }

        public Task ResetAccessFailedCountAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.AccesosFallidos = 0;

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.AccesosFallidos);
        }

        public Task<bool> GetLockoutEnabledAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Activo);
        }

        public Task SetLockoutEnabledAsync(Usuario user, bool enabled) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.Activo = enabled;

            return Task.FromResult(0);
        }

        #endregion


        #region IUserLoginStore

        public Task AddLoginAsync(Usuario user, UserLoginInfo login) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (login == null) {
                throw new ArgumentNullException("login");
            }

            user.Logins.Add(new Login {
                UsuarioID = user.Id,
                ProveedorClave = login.ProviderKey,
                ProveedorLogin = login.LoginProvider
            });

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(Usuario user, UserLoginInfo login) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (login == null) {
                throw new ArgumentNullException("login");
            }

            string provider = login.LoginProvider;
            string key = login.ProviderKey;
            Login loginEntity = user.Logins.SingleOrDefault(l => l.ProveedorLogin == provider && l.ProveedorClave == key);

            if (loginEntity != null) {
                user.Logins.Remove(loginEntity);
            }

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            IList<UserLoginInfo> result = user.Logins.Select(l => new UserLoginInfo(l.ProveedorLogin, l.ProveedorClave)).ToList();

            return Task.FromResult(result);
        }

        public Task<Usuario> FindAsync(UserLoginInfo login) {
            if (login == null) {
                throw new ArgumentNullException("login");
            }

            string provider = login.LoginProvider;
            string key = login.ProviderKey;
            Login userLogin = context.Logins.FirstOrDefault(l => l.ProveedorLogin == provider && l.ProveedorClave == key);

            if (userLogin == null) {
                return Task.FromResult<Usuario>(result: null);
            }

            Usuario user = context.Usuarios.Find(userLogin.UsuarioID);

            return Task.FromResult(user);
        }

        #endregion


        #region IUserPasswordStore

        public Task SetPasswordHashAsync(Usuario user, string passwordHash) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.Clave= passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Clave);
        }

        public Task<bool> HasPasswordAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            bool hasPassword = !string.IsNullOrEmpty(user.Clave);

            return Task.FromResult(hasPassword);
        }

        #endregion


        #region IUserPhoneNumberStore

        public Task SetPhoneNumberAsync(Usuario user, string phoneNumber) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }
            
            user.Movil = phoneNumber;
            
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Movil);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.MovilConfirmado);
        }

        public Task SetPhoneNumberConfirmedAsync(Usuario user, bool confirmed) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.MovilConfirmado = confirmed;

            return Task.FromResult(0);
        }

        #endregion


        #region IUserRoleStore

        /// <summary>
        /// Obligada por IUserRoleStore, aunque con la gestión de perfiles, lo que hace en realidad
        /// es añadir un perfil a un usuario.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="perfil"></param>
        /// <returns></returns>
        public Task AddToRoleAsync(Usuario user, string perfil)
        {
            return AddProfileAsync(user, perfil);
        }

        public Task AddProfileAsync(Usuario user, string perfil)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(perfil))
            {
                throw new ArgumentException("perfil");
            }

            Perfil p = context.Perfiles.Single(perf => perf.Nombre == perfil);
            return AddProfileAsync(user, p);
        }

        public Task AddProfileAsync(Usuario user, long perfilID)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (perfilID < 1)
            {
                throw new ArgumentException("perfilID");
            }

            Perfil p = context.Perfiles.Single(perf => perf.PerfilID == perfilID);
            return AddProfileAsync(user, p);
        }

        public Task<int> AddProfileAsync(Usuario user, Perfil perfil)
        {
            if (perfil != null && user.Perfiles.All(p => p.PerfilID != perfil.PerfilID)) {
                user.Perfiles.Add(perfil);
            }
            return context.SaveChangesAsync();
        }

        public Task RemoveFromRoleAsync(Usuario user, string perfil)
        {
            return RemoveProfileAsync(user, perfil);
        }

        public Task RemoveProfileAsync(Usuario user, string profileName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(profileName))
            {
                throw new ArgumentNullException("profileName");
            }

            Perfil p = context.Perfiles.FirstOrDefault(r => r.Nombre == profileName);

            return RemoveProfileAsync(user, p);
        }

        public Task RemoveProfileAsync(Usuario user, long profileID)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (profileID < 1)
            {
                throw new ArgumentNullException("profileID");
            }

            Perfil p = context.Perfiles.FirstOrDefault(r => r.PerfilID == profileID);

            return RemoveProfileAsync(user, p);
        }

        public Task RemoveProfileAsync(Usuario user, Perfil perfil)
        {
            if (perfil != null && user.Perfiles.Any(r => r.PerfilID == perfil.PerfilID))
            {
                user.Perfiles.Remove(perfil);
            }
            return context.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            IEnumerable<string> rolesUnicos = user.Perfiles             // Lista de perfiles del usuario
                                  .Select(p => p.Roles) // Lista de listas de roles de cada perfil
                                  .SelectMany(x => x)   // Convertir a una lista de roles
                                  .Select(x => x.Name)  // Nos quedamos sólo con el nombre del rol
                                  .Distinct();          // Quitar duplicados

            return Task.FromResult<IList<string>>(rolesUnicos.ToList());
        }

        public async Task<bool> IsInRoleAsync(Usuario user, string roleName) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName)) {
                throw new ArgumentNullException("roleName");
            }

            bool isInRole = (await GetRolesAsync(user)).Any(r => r == roleName);
            return isInRole;
        }

        #endregion


        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(Usuario user, string stamp) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.MarcaSeguridad = stamp;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.MarcaSeguridad);
        }

        #endregion


        #region IUserStore

        public void Dispose() {
            if (context == null) {
                return;
            }

            if (isDisposable) {
                ((IDisposable) context).Dispose();
            }

            context = null;
        }

        public Task CreateAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            if (user.Creado == DateTime.MinValue) {
                user.Creado = DateTime.Now;
            }

            user.Actualizado = DateTime.Now;

            context.Usuarios.Add(user);
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            Usuario userModificar = context.Usuarios.Single(u => u.UsuarioID == user.UsuarioID);
            // TODO actualizar el resto de campos
            userModificar.Actualizado = DateTime.Now;
            return context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            context.Usuarios.Remove(user);
            await context.SaveChangesAsync();
        }

        public Task<Usuario> FindByIdAsync(long userId) {
            Usuario user = context.Usuarios.FirstOrDefault(u => u.UsuarioID == userId);            
            return Task.FromResult(user);
        }

        public Task<Usuario> FindByNameAsync(string userName) {
            if (string.IsNullOrEmpty(userName)) {
                throw new ArgumentNullException("userName");
            }

            Usuario user = context.Usuarios.FirstOrDefault(u => u.Email == userName);            
            return Task.FromResult(user);
        }

        #endregion


        #region IUserTwoFactorStore

        public Task SetTwoFactorEnabledAsync(Usuario user, bool enabled) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            user.DosFactoresHabilitado = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(Usuario user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.DosFactoresHabilitado);
        }

        #endregion


        #region IQueryableUserStore

        public IQueryable<Usuario> Users {
            get { return context.Usuarios; }
        }

        #endregion


        #region Private functions

        private PushNews.Dominio.Entidades.Claim GetUserClaim(IUser<long> user, Claim claim) {
            return context.Claims.SingleOrDefault(c => c.Tipo == claim.Type && c.Valor == claim.Value && c.UsuarioID == user.Id);
        }

        Task IUserClaimStore<Usuario, long>.AddClaimAsync(Usuario user, Claim claim)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
