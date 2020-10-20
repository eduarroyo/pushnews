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
using System.Linq;
using System.Threading.Tasks;

namespace PushNews.Seguridad
{
    public class RoleStore : IQueryableRoleStore<Rol, long>
    {
        private IPushNewsUnitOfWork context;
        private readonly bool isDisposable;

        public RoleStore(IPushNewsUnitOfWork context)
        {
            if (context == null) {
                throw new ArgumentNullException(paramName: "context");
            }

            this.context = context;
            isDisposable = false;
        }

        //static RoleStore() {
        //    FacturasModel.SynchronizeSchema();
        //}

        #region IRoleStore        

        public void Dispose()
        {
            if (context == null)
            {
                return;
            }

            if (isDisposable) {
                context.Dispose();
            }

            context = null;
        }

        public async Task CreateAsync(Rol rol) {
            if (rol == null) {
                throw new ArgumentNullException(paramName: "role");
            }

            context.Roles.Add(rol);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rol rol)
        {
            if (rol == null) {
                throw new ArgumentNullException(paramName: "role");
            }

            Rol rolModificar = context.Roles.Single(r => r.RolID == rol.RolID);
            rolModificar.Name = rol.Name;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rol role)
        {
            if (role == null) {
                throw new ArgumentNullException(paramName: "role");
            }

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
        }

        public async Task<Rol> FindByIdAsync(long rolID)
        {
            Rol rol = context.Roles.FirstOrDefault(r => r.Id.Equals(rolID));
            return await Task.FromResult(rol);
        }

        public Task<Rol> FindByNameAsync(string nombreRol)
        {
            Rol rol = context.Roles.FirstOrDefault(r => r.Name == nombreRol);
            return Task.FromResult(rol);
        }

        #endregion

        #region IQueryableRoleStore

        public IQueryable<Rol> Roles
        {
            get { return context.Roles; }
        }

        #endregion

        public async Task GuardarCambiosAsync()
        {
            await context.SaveChangesAsync();
        }

        public void GuardarCambios()
        {
            context.SaveChanges();
        }
    }
}
