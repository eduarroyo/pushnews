using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Rutas;
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
    public class RutasController : BaseController
    {
        public RutasController(): base()
        { }

        [Authorize(Roles="LeerRutas")]
        public ActionResult Index()
        {
            IHermandadesServicio hSrv = Servicios.HermandadesServicio();
            ViewBag.Hermandades = hSrv.Get(h => h.Activo, hh => hh.OrderBy(h => h.Nombre))
                                      .Select(h => new SelectListItem()
                                      {
                                          Value = h.HermandadID.ToString(),
                                          Text = h.Nombre
                                      });

            IGpssServicio gpsSrv = Servicios.GpssServicio();
            ViewBag.Gpss = gpsSrv.Get(g => g.Activo, gg => gg.OrderBy(g => g.Matricula))
                                 .Select(g => new SelectListItem()
                                 {
                                     Value = g.GpsID.ToString(),
                                     Text = $"{g.Matricula} (ApiID={g.GpsApiID})"
                                 });

            ViewBag.UrlMapas = ObtenerPlantillaUrlMapas();
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Rutas");
        }

        [Authorize(Roles = "LeerRutas")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.RutasServicio();
            var registros = srv.Get(includeProperties: "GpsCabeza, GpsCola, Hermandad")
                .Select(r => RutaModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarRutas")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarRutas")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request,
            [Bind(Exclude = "CalculoTiempo, CalculoVelocidad, CalculoDistancia")] RutaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.RutasServicio();
                var eliminar = srv.GetSingle(p => p.RutaID == model.RutaID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar ruta: {Txt.Rutas.ArtEntidad} con id={model.RutaID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Rutas.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Rutas.ArtEntidad} con id={model.RutaID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Rutas.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarRutas")]
        public ActionResult Modificar([DataSourceRequest] DataSourceRequest request,
            [Bind(Exclude = "CalculoTiempo, CalculoVelocidad, CalculoDistancia")] RutaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.RutasServicio();
                    var modificar = srv.GetSingle(p => p.RutaID == model.RutaID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        srv.Update(modificar);
                        result = new[] { RutaModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Rutas.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar la ruta con id={model.RutaID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Rutas.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearRutas")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request,
            [Bind(Exclude = "CalculoTiempo, CalculoVelocidad, CalculoDistancia")] RutaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.RutasServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { RutaModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir la ruta {model.Descripcion}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Rutas.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }
    }
}