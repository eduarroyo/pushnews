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
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PushNews.Seguridad
{
    public class UserStore : IPushNewsUserStore<Usuario, Rol, long>
    {
        private IPushNewsUnitOfWork context;
        private readonly bool isDisposable;

        public UserStore(IPushNewsUnitOfWork context)
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

        public bool IsInRole(Usuario user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            bool isInRole = user.Rol.Name == roleName;
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

        public void ActualizarPerfiles(Usuario usuario, IEnumerable<long> perfilesAniadir, IEnumerable<long> perfilesQuitar)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Rol role)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Rol role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Rol role)
        {
            throw new NotImplementedException();
        }

        Task<Rol> IRoleStore<Rol, long>.FindByIdAsync(long roleId)
        {
            throw new NotImplementedException();
        }

        Task<Rol> IRoleStore<Rol, long>.FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public void AsignarRol(Usuario usuario, long rolId)
        {
            usuario.RolID = rolId;
        }

        #endregion


        #region IQueryableUserStore

        public IQueryable<Usuario> Users
        {
            get { return context.Usuarios; }
        }

        public IQueryable<Rol> Roles
        { 
            get { return context.Roles; }
        }

        #endregion
    }
}
