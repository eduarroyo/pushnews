using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Parametros;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Parametros;
using PushNews.Negocio.Interfaces;
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
    public class ParametrosController : BaseController
    {
        public ParametrosController(): base()
        { }

        [Authorize(Roles="Administrador")]
        public ActionResult Index()
        {
            IUsuariosServicio srv = Servicios.UsuariosServicio();
            long userID = CurrentUserID();
            Usuario usr = srv.GetSingle(u => u.UsuarioID == userID);
            List<SelectListItem> apps = usr.Aplicaciones
                .Select(a => new SelectListItem { Value = a.AplicacionID.ToString(), Text = a.Nombre })
                .ToList();
            apps.Insert(0, new SelectListItem { Value = "0", Text = Txt.Comun.Ninguna });
            ViewBag.Aplicaciones = apps;
            return PartialView("Parametros");
        }

        [Authorize(Roles="Administrador")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.ParametrosServicio();
            var registros = srv.Get()
                .Select(r => ParametroGrid.FromEntity(r));
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles="Administrador")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles="Administrador")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, ParametroGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.ParametrosServicio();
                var eliminar = srv.GetSingle(p => p.ParametroID == model.ParametroID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug("Eliminar parámetro: el parámetro con id=" + model.ParametroID + " no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Parametros.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error("Error al eliminar el parámetro con id=" + model.ParametroID, e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Parametros.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles="Administrador")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, ParametroGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.ParametrosServicio();
                    var modificar = srv.GetSingle(p => p.ParametroID == model.ParametroID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { ParametroGrid.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Parametros.ArtEntidad).Frase() };
                    }
                }
                catch (ParametroExisteException pee)
                {
                    log.Error($"Error al modificar {Txt.Parametros.ArtEntidad}. Usuario: {CurrentUserID()}", pee);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + " " + pee.Message, Txt.Parametros.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error("Error al modificar el parámetro con id=" + model.ParametroID, e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Parametros.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles="Administrador")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, ParametroGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.ParametrosServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { ParametroGrid.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (ParametroExisteException pee)
                {
                    log.Error($"Error al añadir {Txt.Parametros.ArtEntidad}. Usuario: {CurrentUserID()}", pee);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + " " + pee.Message, Txt.Parametros.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error("Error al añadir el parámetro " + model.Nombre, e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Parametros.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de motivos de baja.
        /// </summary>
        [Authorize(Roles="Administrador")]
        public ActionResult ListaParametros()
        {
            var srv = Servicios.ParametrosServicio();
            var consulta = srv.Get();
            var registros = consulta.Select(l => ParametroGrid.FromEntity(l));

            // Creamos el modelo de lista de selección a partir del resultado obtenido de la 
            // consulta. Si se ha especificado un país, los grupos se establecen por 
            // Pais/Provincia. Si no, sólo por Provincia.
            var lista = new SelectList(registros, "ParametroID", "Nombre");
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}