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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;

namespace PushNews.Seguridad
{
    public class RoleStore : IPushNewsRoleProfileStore<Rol, Perfil, long>
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
            rolModificar.Modulo = rol.Modulo;
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

        #region IQueryableRoleProfileStore
        public IQueryable<Perfil> Perfiles()
        {
            return context.Perfiles;
        }

        public Perfil Perfil(long perfilID)
        {
            return context.Perfiles.SingleOrDefault(p => p.PerfilID == perfilID);
        }

        public Perfil NuevoPerfil()
        {
            return context.Perfiles.Create();
        }

        public void NuevoPerfil(Perfil perfil)
        {
            context.Perfiles.Add(perfil);
        }

        public void EliminarPerfil(Perfil perfil)
        {
            context.Perfiles.Remove(perfil);
        }

        public void RenombrarPerfil(Perfil perfil, string nuevoNombre)
        {
            if (perfil == null)
            {
                throw new ArgumentNullException(paramName: "perfil");
            }
            if(string.IsNullOrWhiteSpace(nuevoNombre))
            {
                throw new ArgumentException(message: "El nuevo nombre para el perfil no es válido.",
                                            paramName: "nuevoNombre");
            }

            if(perfil != null)
            {
                perfil.Nombre = nuevoNombre;
            }
        }

        public void AniadirRol(Perfil perfil, long rolID)
        {
            Rol rol = context.Roles.Single(r => r.RolID == rolID);
            perfil.Roles.Add(rol);
        }

        public void QuitarRol(Perfil perfil, long rolID)
        {
            if (perfil == null)
            {
                throw new ArgumentNullException(paramName: "perfil");
            }

            Rol rol = perfil.Roles.Single(r => r.RolID == rolID);
            perfil.Roles.Remove(rol);
        }

        public void AniadirRoles(Perfil perfil, IEnumerable<long> roles)
        {
            if (perfil == null)
            {
                throw new ArgumentNullException(paramName: "perfil");
            }
            IQueryable<Rol> rolesNuevos = context.Roles.Where(r => roles.Contains(r.RolID));
            foreach (var rol in rolesNuevos)
            {
                perfil.Roles.Add(rol);   
            }
        }

        public void QuitarRoles(Perfil perfil, IEnumerable<long> roles)
        {
            foreach(var rolID in roles)
            {
                Rol rolQuitar = perfil.Roles.SingleOrDefault(r => r.RolID == rolID);
                if(rolQuitar != null)
                {
                    perfil.Roles.Remove(rolQuitar);
                }
            }
        }

        public async Task GuardarCambiosAsync()
        {
            await context.SaveChangesAsync();
        }

        public void GuardarCambios()
        {
            context.SaveChanges();
        }

        #endregion

        public async Task ActualizarRolesAsync (Perfil perfil, IEnumerable<long> rolesAniadir, IEnumerable<long> rolesQuitar)
        {
            Perfil per = context.Perfiles.Single(e => e.PerfilID == perfil.PerfilID);

            if (rolesQuitar != null && rolesQuitar.Any())
            {
                foreach (var rq in rolesQuitar)
                {
                    Rol rQuitar = per.Roles?.SingleOrDefault(p => p.RolID == rq);
                    if (rQuitar != null)
                    {
                        per.Roles.Remove(rQuitar);
                    }
                }
            }
            if (rolesAniadir != null && rolesAniadir.Any())
            {
                IQueryable<Rol> rAniadir = context.Roles.Where(r => rolesAniadir.Contains(r.RolID));
                foreach (var r in rAniadir)
                {
                    if(per.Roles == null)
                    {
                        per.Roles = new List<Rol>();
                    }
                    per.Roles.Add(r);
                }
            }
            await context.SaveChangesAsync();
        }

        public void ActualizarRoles(Perfil perfil, IEnumerable<long> rolesAniadir, IEnumerable<long> rolesQuitar)
        {
            Perfil per = context.Perfiles.Single(e => e.PerfilID == perfil.PerfilID);

            if (rolesQuitar != null && rolesQuitar.Any())
            {
                foreach (var rq in rolesQuitar)
                {
                    Rol rQuitar = per.Roles.SingleOrDefault(p => p.RolID == rq);
                    if (rQuitar != null)
                    {
                        per.Roles.Remove(rQuitar);
                    }
                }
            }
            if (rolesAniadir != null && rolesAniadir.Any())
            {
                IQueryable<Rol> rAniadir = context.Roles.Where(r => rolesAniadir.Contains(r.RolID));
                foreach (var r in rAniadir)
                {
                    per.Roles.Add(r);
                }
            }

            context.SaveChanges();
        }
    }
}
