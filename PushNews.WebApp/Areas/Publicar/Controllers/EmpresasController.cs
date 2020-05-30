using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Empresas;
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
    public class EmpresasController : BaseController
    {
        public EmpresasController() : base()
        { }

        [Authorize(Roles = "LeerEmpresas")]
        public ActionResult Index()
        {
            ViewBag.UrlMapas = ObtenerPlantillaUrlMapas();
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Empresas");
        }

        [Authorize(Roles = "LeerEmpresas")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.EmpresasServicio();
            var registros = srv.Get()
                .Select(r => EmpresaModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarEmpresas")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarEmpresas")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, EmpresaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                var srv = Servicios.EmpresasServicio();
                var eliminar = srv.GetSingle(p => p.EmpresaID == model.EmpresaID);
                if (eliminar != null)
                {
                    srv.Delete(eliminar);
                    await srv.ApplyChangesAsync();
                }
                else
                {
                    log.Debug($"Eliminar empresa: {Txt.Empresas.ArtEntidad} con id={model.EmpresaID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Empresas.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Empresas.ArtEntidad} con id={model.EmpresaID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Empresas.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarEmpresas")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, EmpresaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.EmpresasServicio();
                    var modificar = srv.GetSingle(p => p.EmpresaID == model.EmpresaID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { EmpresaModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Empresas.ArtEntidad).Frase() };
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar la empresa con id={model.EmpresaID}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Empresas.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "CrearEmpresas")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, EmpresaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.EmpresasServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    srv.Insert(nuevo);
                    await srv.ApplyChangesAsync();
                    result = new[] { EmpresaModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir la empresa {model.Nombre}", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Empresas.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }
    }
}