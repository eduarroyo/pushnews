using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Telefonos;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Publicar.Controllers
{
    [Authorize]
    public class TelefonosController : BaseController
    {
        public TelefonosController(): base()
        { }

        [Authorize(Roles="LeerTelefonos")]
        public ActionResult Index()
        {
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Telefonos");
        }

        [Authorize(Roles = "LeerTelefonos")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.TelefonosServicio();
            var registros = srv.Get()
                .Select(r => TelefonoModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarTelefonos")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarTelefonos")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, TelefonoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.TelefonosServicio();
                var eliminar = srv.GetSingle(p => p.TelefonoID == model.TelefonoID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar teléfono: {Txt.Telefonos.ArtEntidad} con id={model.TelefonoID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Telefonos.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Telefonos.ArtEntidad} con id={model.TelefonoID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Telefonos.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarTelefonos")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, TelefonoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.TelefonosServicio();
                    var modificar = srv.GetSingle(p => p.TelefonoID == model.TelefonoID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { TelefonoModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Telefonos.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar el teléfono con id={model.TelefonoID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Telefonos.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearTelefonos")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, TelefonoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.TelefonosServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { TelefonoModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir el teléfono {model.Descripcion}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Telefonos.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }
    }
}