using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Localizaciones;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class LocalizacionesController : BaseController
    {
        public LocalizacionesController(): base()
        { }

        [Authorize(Roles="LeerLocalizaciones")]
        public ActionResult Index()
        {
            double[] coordenadas = CoordenadasPorDefecto();
            ViewBag.Latitud = coordenadas[0];
            ViewBag.Longitud = coordenadas[1];
            return PartialView("Localizaciones");
        }

        [Authorize(Roles = "LeerLocalizaciones")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.LocalizacionesServicio();
            var registros = srv.Get()
                .Select(r => LocalizacionModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarLocalizaciones")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarLocalizaciones")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, LocalizacionModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.LocalizacionesServicio();
                    var modificar = srv.GetSingle(p => p.LocalizacionID == model.LocalizacionID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { LocalizacionModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Localizaciones.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar el localización con id={model.LocalizacionID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Localizaciones.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarLocalizaciones")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, LocalizacionModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.LocalizacionesServicio();
                var eliminar = srv.GetSingle(p => p.LocalizacionID == model.LocalizacionID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar localización: {Txt.Localizaciones.ArtEntidad} con id={model.LocalizacionID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Localizaciones.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Localizaciones.ArtEntidad} con id={model.LocalizacionID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Localizaciones.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearLocalizaciones")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, LocalizacionModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.LocalizacionesServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { LocalizacionModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir la localización {model.Descripcion}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Localizaciones.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }
    }
}