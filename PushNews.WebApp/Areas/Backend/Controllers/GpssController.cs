using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Gpss;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
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
    public class GpssController : BaseController
    {
        public GpssController(): base()
        { }

        [Authorize(Roles="LeerGpss")]
        public ActionResult Index()
        {
            ViewBag.UrlMapas = ObtenerPlantillaUrlMapas();

            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Gpss");
        }

        [Authorize(Roles = "LeerGpss")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            double bateriaMinimo = ObtenerMinimoBateria();

            var srv = Servicios.GpssServicio();
            var registros = srv.Get()
                .Select(r => GpsModel.FromEntity(r, bateriaMinimo))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private double ObtenerMinimoBateria()
        {
            double bateriaMinimo = 10;
            var pSrv = Servicios.ParametrosServicio();
            Parametro pBateriaMinimo = pSrv.GetByName("GpsBateriaMinimo");

            // Si el parámetro no existe o tiene un valor que no se corresponde con un número, 
            // se utiliza el valor por defecto 10.
            bateriaMinimo = double.TryParse((pBateriaMinimo?.Valor ?? "10"), out bateriaMinimo)
                ? bateriaMinimo : 10;

            return bateriaMinimo;
        }

        [HttpPost]
        [Authorize(Roles = "ExportarGpss")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarGpss")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, GpsModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.GpssServicio();
                var eliminar = srv.GetSingle(p => p.GpsID == model.GpsID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar gps: {Txt.Gpss.ArtEntidad} con id={model.GpsID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Gpss.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Gpss.ArtEntidad} con id={model.GpsID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Gpss.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarGpss")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, GpsModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    double bateriaMinimo = ObtenerMinimoBateria();
                    var srv = Servicios.GpssServicio();
                    var modificar = srv.GetSingle(p => p.GpsID == model.GpsID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { GpsModel.FromEntity(modificar, bateriaMinimo) }
                                    .ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Gpss.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar el gps con id={model.GpsID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Gpss.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearGpss")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, GpsModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    double bateriaMinimo = ObtenerMinimoBateria();
                    var srv = Servicios.GpssServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { GpsModel.FromEntity(nuevo, bateriaMinimo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir el gps {model.Matricula}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Gpss.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [Authorize(Roles = "LeerGpss")]
        public async Task<ActionResult> Lista()
        {
            var srv = Servicios.GpssServicio();
            var registros = (await srv.GetAsync(h => h.Activo, h => h.OrderBy(hh => hh.Matricula)))
                .Select(g => new SelectListItem()
                {
                    Value = g.GpsID.ToString(),
                    Text = $"{g.Matricula} (ApiID={g.GpsApiID})"
                })
                .ToList();

            return Json(registros, JsonRequestBehavior.AllowGet);
        }
    }
}