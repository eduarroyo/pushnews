﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using PushNews.Negocio;
using PushNews.Dominio;
using PushNews.Negocio.Interfaces;
using Txt = PushNews.WebApp.App_LocalResources;
using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Models.UI
{
    public static class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Arguments", "JustCode_LiteralArgumentIsNotNamedDiagnostic:The used literal argument is not named", Justification = "<Pendiente>")]
        private static List<ElementoMenu> Elementos(IEnumerable<Dominio.Enums.AplicacionCaracteristica> caracteristicas,
            bool ocultarCategorias = false, bool ocultarTelefonos = false, bool ocultarLocalizaciones = false)
        {
            // Flags correspondientes a características de la aplicación.
            bool asociados = caracteristicas.Contains(Dominio.Enums.AplicacionCaracteristica.Asociados);
            bool empresas = caracteristicas.Contains(Dominio.Enums.AplicacionCaracteristica.Empresas);
            bool hermandades = caracteristicas.Contains(Dominio.Enums.AplicacionCaracteristica.Hermandades);

            // Elementos del submenú Comunicaciones.
            var Comunicaciones = new List<ElementoMenu>();
            Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Comunicaciones, "Publicar", "comunicaciones", "LeerComunicaciones", ""));

            if (!ocultarCategorias)
            {
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Categorias, "Publicar", "categorias", "LeerCategorias", ""));
            }

            if (!ocultarTelefonos)
            {
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Telefonos, "Publicar", "telefonos", "LeerTelefonos", ""));
            }

            if (!ocultarLocalizaciones)
            {
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Localizaciones, "Publicar", "localizaciones", "LeerLocalizaciones", ""));
            }

            if (empresas)
            {
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Empresas, "Publicar", "empresas", "LeerEmpresas", ""));
            }

            if (hermandades)
            {
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Hermandades, "Publicar", "hermandades", "LeerHermandades", ""));
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Rutas, "Publicar", "rutas", "LeerRutas", ""));
                Comunicaciones.Add(new ElementoMenu(Txt.Secciones.Gpss, "Publicar", "gpss", "LeerGpss", ""));
            }

            // Elementos del submenú Administración.
            var Administracion = new List<ElementoMenu>();
            Administracion.Add(new ElementoMenu(Txt.Secciones.Aplicaciones, "Publicar", "aplicaciones", "LeerAplicaciones", ""));
            if(asociados)
            {
                Administracion.Add(new ElementoMenu(Txt.Secciones.Asociados, "Publicar", "asociados", "LeerAsociados", ""));
            }
            Administracion.Add(new ElementoMenu(Txt.Secciones.AplicacionesCaracteristicas, "Publicar", "aplicacionesCaracteristicas", "LeerAplicacionesCaracteristicas", ""));
            Administracion.Add(new ElementoMenu(Txt.Secciones.Usuarios, "Publicar", "usuarios", "LeerUsuarios", ""));
            Administracion.Add(new ElementoMenu(Txt.Secciones.Parametros, "Publicar", "parametros", "LeerParametros", ""));

            // Árbol del menú
            return new List<ElementoMenu>()
            {
                new ElementoMenu(Txt.Secciones.Escritorio, "Publicar", "escritorio", "", "fa fa-home"),
                new ElementoMenu(Txt.Secciones.Comunicaciones, "fa fa-newspaper-o", Comunicaciones),
                new ElementoMenu(Txt.Secciones.Administracion, "fa fa-cog", Administracion)
            };
        }

        public static IEnumerable<ElementoMenu> GenerarMenu(long usuarioID, Aplicacion aplicacion)
        {
            IPushNewsUnitOfWork uow = HttpContext.Current.Request.GetOwinContext().Get<IPushNewsUnitOfWork>();
            var servicios = new ServiciosFactoria(uow, aplicacion);
            IAplicacionesServicio appSrv = servicios.AplicacionesServicio();
            Aplicacion app = appSrv.GetSingle(a => a.AplicacionID == aplicacion.AplicacionID);
            IUsuariosServicio srv = servicios.UsuariosServicio();

            List<string> roles = srv.GetSingle(e => e.UsuarioID == usuarioID)
                .Perfiles.Select(p => p.Roles)
                .SelectMany(r => r).Select(r => r.Nombre)
                .Distinct()
                .ToList();

            // Flags para ocultar categorías, teléfonos y localizaciones en el menú.
            // Los tres elementos se mostrarán siempre si el usuario es administrador, independientemente de
            // si tiene o no tiene categorías asignadas.
            // Si el usuario no es administrador, los tres elementos se mostrarán si no tiene ninguna 
            // categoría asignada en la aplicación acutal. Si tiene al menos una categoría asignada en la 
            // aplicación acutal, los tres elementos se ocultan.
            IUsuariosServicio usuariosServicio = servicios.UsuariosServicio();
            Usuario usuario = usuariosServicio.GetSingle(u => u.UsuarioID == usuarioID);
            bool usuarioEsAdmin = usuario.Perfiles.Any(p => p.Nombre == "Administrador");
            bool ocultarCategorias = !usuarioEsAdmin && usuario.Categorias.Where(a => a.AplicacionID == aplicacion.AplicacionID).Any();
            bool ocultarTelefonos = !usuarioEsAdmin && ocultarCategorias ;
            bool ocultarLocalizaciones = !usuarioEsAdmin && ocultarCategorias;

            IEnumerable<Dominio.Enums.AplicacionCaracteristica> caracteristicas = app.Caracteristicas
                .Where(c => c.Activo)
                .Select(c => (Dominio.Enums.AplicacionCaracteristica)c.AplicacionCaracteristicaID);
            List<ElementoMenu> menuCompleto = Elementos(caracteristicas, ocultarCategorias, ocultarTelefonos, ocultarLocalizaciones);
            var menuFiltrado = new List<ElementoMenu>();

            foreach(var eActual in menuCompleto)
            {
                if(eActual.Roles.Any(r => r.Length == 0 || roles.Contains(r)))
                {
                    if(eActual.TieneHijos)
                    {
                        QuitarHijos(eActual, roles);
                    }
                    menuFiltrado.Add(eActual);
                }
            }

            return menuFiltrado;
        }

        private static void QuitarHijos(ElementoMenu submenu, IEnumerable<string> roles)
        {
            var hijosQuitar = new List<ElementoMenu>();

            foreach(var hijo in submenu.Hijos)
            {
                if(hijo.Roles.Any(r => roles.Contains(r)))
                {
                    if(hijo.TieneHijos)
                    {
                        QuitarHijos(hijo, roles);
                    }                    
                }
                else
                {
                    hijosQuitar.Add(hijo);
                }
            }

            foreach(var quitar in hijosQuitar)
            {
                submenu.Hijos.Remove(quitar);
            }

        }
    }
}
