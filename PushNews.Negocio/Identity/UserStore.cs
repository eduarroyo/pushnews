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

namespace PushNews.Negocio.Identity
{
    public class UserStore :
        IUserPasswordStore<Usuario, long>,
        IUserEmailStore<Usuario, long>,
        IUserRoleStore<Usuario, long>
    {
        private IPushNewsUnitOfWork context;

        public UserStore(IPushNewsUnitOfWork context)
        {
            this.context = context;
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
        public Task SetPasswordHashAsync(Usuario user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Clave = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Clave);
        }

        public Task<bool> HasPasswordAsync(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            bool hasPassword = !string.IsNullOrEmpty(user.Clave);

            return Task.FromResult(hasPassword);
        }

        public void Dispose()
        {
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

            bool isInRole = GetRoles(user).Contains(roleName);
            return isInRole;
        }

        public async Task<bool> IsInRoleAsync(Usuario user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            bool isInRole = (await GetRolesAsync(user)).Any(r => r == roleName);
            return isInRole;
        }

        public IList<string> GetRoles(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Rol r = context.Roles.Single(rr => rr.RolID == user.RolID);
            return new List<string> { r.Name };
        }

        public Task<IList<string>> GetRolesAsync(Usuario user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Rol r = context.Roles.Single(rr => rr.RolID == user.RolID);
            return Task.FromResult<IList<string>>(new List<string> { r.Name });
        }


        public async Task<IdentityResult> AsignarRol(long usuarioId, long rolId)
        {
            Usuario u = await FindByIdAsync(usuarioId);
            if (u == null)
            {
                return IdentityResult.Failed("Usuario no encontrado");
            }
            return await AsignarRol(u, rolId);
        }

        public async Task<IdentityResult> AsignarRol(Usuario usuario, long rolId)
        {
            usuario.RolID = rolId;
            await SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AsignarRol(Usuario usuario, string rol)
        {
            Rol r = context.Roles.SingleOrDefault(rr => rr.Nombre == rol);

            if (rol == null)
            {
                return IdentityResult.Failed("Rol no encontrado");
            }

            usuario.RolID = r.RolID;
            await SaveChangesAsync();
            return IdentityResult.Success;
        }

        public Task AddToRoleAsync(Usuario user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(Usuario user, string roleName)
        {
            throw new NotImplementedException();
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


        #region IUserStore

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
    }
}
