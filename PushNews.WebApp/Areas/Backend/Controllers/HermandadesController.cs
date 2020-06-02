using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Hermandades;
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
    public class HermandadesController : BaseController
    {
        public HermandadesController() : base()
        { }

        [Authorize(Roles = "LeerHermandades")]
        public ActionResult Index()
        {
            ViewBag.UrlMapas = ObtenerPlantillaUrlMapas();
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Hermandades");
        }

        [Authorize(Roles = "LeerHermandades")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.HermandadesServicio();
            var registros = srv.Get()
                .Select(r => HermandadModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarHermandades")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarHermandades")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, HermandadModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.HermandadesServicio();
                var eliminar = srv.GetSingle(p => p.HermandadID == model.HermandadID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar hermandad: {Txt.Hermandades.ArtEntidad} con id={model.HermandadID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Hermandades.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Hermandades.ArtEntidad} con id={model.HermandadID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Hermandades.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarHermandades")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, HermandadModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.HermandadesServicio();
                    var modificar = srv.GetSingle(p => p.HermandadID == model.HermandadID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { HermandadModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Hermandades.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar la hermandad con id={model.HermandadID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Hermandades.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearHermandades")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, HermandadModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.HermandadesServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { HermandadModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir la hermandad {model.Nombre}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Hermandades.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }


        [Authorize(Roles = "LeerHermandades")]
        public async Task<ActionResult> Lista()
        {
            var srv = Servicios.HermandadesServicio();
            var registros = (await srv.GetAsync(h => h.Activo, h => h.OrderBy(hh => hh.Nombre)))
                .Select(h => new SelectListItem()
                {
                    Value = h.HermandadID.ToString(),
                    Text = h.Nombre
                })
                .ToList();

            return Json(registros, JsonRequestBehavior.AllowGet);
        }
    }
}