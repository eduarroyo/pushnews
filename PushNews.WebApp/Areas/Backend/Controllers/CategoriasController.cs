using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Categorias;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class CategoriasController : BaseController
    {
        public CategoriasController(): base()
        { }

        [Authorize]
        public ActionResult Index()
        {
            return PartialView("Categorias");
        }

        [Authorize]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.CategoriasServicio();
            var registros = srv.Get()
                .Select(r => CategoriaModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, CategoriaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState); ;

            // Para poder modificar una categoría, el usuario debe ser administrador o bien no tener ninguna
            // categoría asignada de la aplicación.
            if (Usuario.Rol.Nombre == "Administrador" || !Usuario.Categorias.Any(c => c.AplicacionID == Aplicacion.AplicacionID))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var srv = Servicios.CategoriasServicio();
                        var modificar = srv.GetSingle(p => p.CategoriaID == model.CategoriaID);
                        if (modificar != null)
                        {
                            srv.CambiarOrdenCategoria(model.CategoriaID, model.Orden);
                            model.ActualizarEntidad(modificar);
                            await srv.ApplyChangesAsync();
                            result = new[] { CategoriaModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                        }
                        else
                        {
                            result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Categorias.ArtEntidad).Frase() };
                        }
                    }
                    catch (CategoriaExisteException cee)
                    {
                        log.Error($"Error al modificar {Txt.Categorias.ArtEntidad}. Usuario: {CurrentUserID()}", cee);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + cee.Message, Txt.Categorias.ArtEntidad).Frase() };
                    }
                    catch (Exception e)
                    {
                        log.Error("Error al modificar el categoría con id=" + model.CategoriaID, e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Categorias.ArtEntidad).Frase() };
                    }
                }
            }
            else
            {
                result.Errors = new[] { Txt.Categorias.NoPermitido };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, CategoriaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState); ;
            if (Usuario.Categorias.Any())
            {
                result.Errors = new[] { Txt.Categorias.NoPermitido };
            }
            else
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var srv = Servicios.CategoriasServicio();
                        var nuevo = srv.Create();
                        model.ActualizarEntidad(nuevo);
                        nuevo.UsuarioID = CurrentUserID();
                        srv.Insert(nuevo);
                        await srv.ApplyChangesAsync();
                        result = new[] { CategoriaModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                    }
                    catch (CategoriaExisteException cee)
                    {
                        log.Error($"Error al añadir {Txt.Categorias.ArtEntidad}. Usuario: {CurrentUserID()}", cee);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + cee.Message, Txt.Categorias.ArtEntidad).Frase() };
                    }
                    catch (Exception e)
                    {
                        log.Error("Error al añadir el categoría " + model.Nombre, e);
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Categorias.ArtEntidad).Frase() };
                    }
                }
            }
            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de categorías.
        /// </summary>
        [Authorize]
        public ActionResult ListaCategorias()
        {
            var srv = Servicios.CategoriasServicio();
            var registros = srv
                .ListaCategorias()
                .Select(CategoriaModel.FromEntity);
            return Json(registros, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Proporciona la lista de categorías activas y autorizadas al usuario de la sesión.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CategoriasUsuario()
        {
            long aplicacionActualId = Aplicacion.AplicacionID;
            // Categorias del usuario y la aplicación actual, activas e inactivas.
            IEnumerable<Categoria> registros = Usuario.Categorias
                .Where(c => c.AplicacionID == aplicacionActualId);

            // Si el usuario tiene alguna categoría asignada y no es administrador, hay que quedarse sólo con
            // las que estén activas.
            if(registros.Any() && Usuario.Rol.Nombre != "Administrador")
            {
                registros = registros.Where(c => c.Activo);
            }

            // Si no tiene ninguna categoría asignada (activa o inactiva) o bien es administrador, significa
            // que puede usar cualquiera, siempre que esté activa.
            else
            {
                var srv = Servicios.CategoriasServicio();
                registros = srv.ListaCategorias();
            }

            IEnumerable<CategoriaModel> categorias = registros
                .Select(CategoriaModel.FromEntity);
            return Json(categorias, JsonRequestBehavior.AllowGet);
        }
    }
}