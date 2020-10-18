using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Account;
using PushNews.WebApp.Models.Usuarios;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class UsuariosController : BaseController
    {
        public UsuariosController(): base()
        { }

        // GET: Usuarios
        [Authorize(Roles="LeerUsuarios")]
        public ActionResult Index()
        {
            ViewBag.Perfiles = new SelectList(RoleManager.Perfiles(), "PerfilID", "Nombre");
            return PartialView("Usuarios");
        }

        [Authorize(Roles="LeerUsuarios")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request, long? aplicacionID = null)
        {
            IUsuariosServicio srv = Servicios.UsuariosServicio();
            IEnumerable<UsuarioGrid> usuarios = srv.Get().Select(emp => UsuarioGrid.FromEntity(emp));
            return Json(usuarios.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarUsuarios")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            byte[] fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }
        
        [HttpPost]
        [Authorize(Roles="ModificarUsuarios")]
        public async Task<ActionResult> CambiarClave(CambiarClaveUsuarioModel model)
        {
            var result = new { errors = new List<string>() };
            if (ModelState.IsValid)
            {
                Usuario usuario = await UserManager.FindByIdAsync(model.UsuarioID);
                if (usuario != null)
                {
                    IdentityResult ir = await UserManager.ChangePasswordAsync(model.UsuarioID, model.Clave);
                    if (!ir.Succeeded)
                    {
                        foreach (var error in ir.Errors)
                        {
                            result.errors.Add(error);
                        }
                    }
                }
                else
                {
                    result.errors.Add(string.Format(Txt.ErroresComunes.NoExiste, Txt.Usuarios.ArtEntidad).Frase());
                }
            }
            else
            {
                // Añadir textos de errores de validación al result.
                result.errors.AddRange(Util.SerializarErroresModelo(ModelState));
            }
            return Json(result);
        }        

        [HttpPost]
        [Authorize(Roles = "ModificarUsuarios")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request,
            UsuarioGrid usuario)
        {
            DataSourceResult result = new[] { usuario }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                bool usuarioActualizado = false;
                try
                {
                    IUsuariosServicio srv = Servicios.UsuariosServicio();
                    Usuario modificar = srv.GetSingle(u => u.UsuarioID == usuario.UsuarioID);
                    if (modificar != null)
                    {
                        if (usuario.CategoriasIDs == null) { usuario.CategoriasIDs = new long[0]; }
                        if (usuario.Aplicaciones == null) { usuario.Aplicaciones = new long[0]; }

                        usuario.ActualizarEntidad(modificar);

                        IEnumerable<long> categoriasActuales = modificar.Categorias.Select(cat => cat.CategoriaID);
                        IEnumerable<long> categoriasAniadirIDs = usuario.CategoriasIDs.Where(cat => !categoriasActuales.Contains(cat));
                        IEnumerable<long> categoriasEliminar = categoriasActuales
                            .Where(cat => !usuario.CategoriasIDs.Contains(cat)).ToArray();
                        foreach (var e in categoriasEliminar)
                        {
                            Categoria cEliminar = modificar.Categorias
                                .Single(eID => eID.CategoriaID == e);
                            modificar.Categorias.Remove(cEliminar);
                        }

                        // Añadir las categorías que vengan en el array de ids de categorías y que no 
                        // tenga ya el usuario.
                        ICategoriasServicio srvCategorias = Servicios.CategoriasServicio();
                        modificar.Categorias
                            .AddRange(srvCategorias.Get(cl => categoriasAniadirIDs.Contains(cl.CategoriaID)));


                        IEnumerable<long> aplicacionesActuales = modificar.Aplicaciones.Select(cl => cl.AplicacionID);
                        IEnumerable<long> aplicacionesAniadirIDs = usuario.Aplicaciones.Where(cl => !aplicacionesActuales.Contains(cl));
                        IEnumerable<long> aplicacionesEliminar = aplicacionesActuales
                            .Where(e => !usuario.Aplicaciones.Contains(e)).ToArray();
                        foreach (var e in aplicacionesEliminar)
                        {
                            Aplicacion aEliminar = modificar.Aplicaciones
                                .Single(eID => eID.AplicacionID == e);
                            modificar.Aplicaciones.Remove(aEliminar);
                        }

                        // Añadir las aplicaciones que vengan en el array de ids de aplicaciones y que no 
                        // tenga ya el usuario.
                        IAplicacionesServicio srvAplicacions = Servicios.AplicacionesServicio();
                        modificar.Aplicaciones
                            .AddRange(srvAplicacions.Get(cl => aplicacionesAniadirIDs.Contains(cl.AplicacionID)));

                        IdentityResult ir = await UserManager.UpdateAsync(modificar);
                        if(ir.Succeeded)
                        {
                            usuarioActualizado = true;
                            await UserManager.EstablecerPerfilesAsync(modificar, new[] { usuario.PerfilID });
                            result = new[] { UsuarioGrid.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                        }
                        else
                        {
                            foreach (var error in ir.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                            result = new[] { usuario }.ToDataSourceResult(request, ModelState);
                        }
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Usuarios.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    if (usuarioActualizado)
                    {
                        log.Error($"Error al establecer perfiles del usuario creado {usuario.Email}", e);
                        result.Errors = new[] { string.Format(Txt.Usuarios.ErrorEstablecerPerfiles, usuario.Email).Frase() };
                    }
                    else
                    {
                        log.Error("Error al modificar el usuario con id=" + usuario.UsuarioID, e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Usuarios.ArtEntidad).Frase() };
                    }
                }
            }

            return Json(result);
        }
        
        [HttpPost]
        [Authorize(Roles = "CrearUsuarios")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, UsuarioGrid usuario)
        {
            if(usuario.CategoriasIDs == null)
            {
                usuario.CategoriasIDs = new long[0];
            }
            DataSourceResult result = new[] { usuario }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                bool usuarioActualizado = false;
                try
                {
                    IAplicacionesServicio apSrv = Servicios.AplicacionesServicio();
                    ICategoriasServicio carSrv = Servicios.CategoriasServicio();
                    IUsuariosServicio empSrv = Servicios.UsuariosServicio();
                    Usuario usuarioNuevo = empSrv.Create();
                    usuario.ActualizarEntidad(usuarioNuevo);
                    usuarioNuevo.Aplicaciones = apSrv.Get(cl => usuario.Aplicaciones.Contains(cl.AplicacionID)).ToList();
                    usuarioNuevo.Categorias = carSrv.Get(cat => usuario.CategoriasIDs.Contains(cat.CategoriaID)).ToList();
                    IdentityResult ir = await UserManager.CreateAsync(usuarioNuevo, usuario.Clave);
                    if (ir.Succeeded)
                    {
                        result = new[] { UsuarioGrid.FromEntity(usuarioNuevo) }.ToDataSourceResult(request, ModelState);
                        usuarioActualizado = true;
                        await UserManager.EstablecerPerfilesAsync(usuarioNuevo, new[] { usuario.PerfilID });
                        result = new[] { UsuarioGrid.FromEntity(usuarioNuevo) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        foreach (var error in ir.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        result = new[] { usuario }.ToDataSourceResult(request, ModelState);
                    }
                }
                catch (Exception e)
                {
                    if (usuarioActualizado)
                    {
                        log.Error($"Error al establecer perfiles del usuario creado {usuario.Email}", e);
                        result.Errors = new[] { string.Format(Txt.Usuarios.ErrorEstablecerPerfiles, usuario.Email).Frase() };
                    }
                    else
                    {
                        log.Error("Error al añadir el usuario " + usuario.Nombre, e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Usuarios.ArtEntidad).Frase() };
                    }
                }
            }

            return Json(result);
        }

        public ActionResult ListaUsuarios()
        {
            IUsuariosServicio srv = Servicios.UsuariosServicio();
            IEnumerable<SelectListItem> usuarios = srv.Get()
                .Where(e => e.Aplicaciones.Any(cl => cl.AplicacionID == Aplicacion.AplicacionID))
                .Select(e => new SelectListItem() 
                    {
                        Value = e.UsuarioID.ToString(),
                        Text = e.ApellidosNombre
                    });

            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }
    }
}