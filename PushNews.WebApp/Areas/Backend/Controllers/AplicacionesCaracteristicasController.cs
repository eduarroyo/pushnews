using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.AplicacionesCaracteristicas;
using PushNews.Negocio.Interfaces;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class AplicacionesCaracteristicasController : BaseController
    {
        public AplicacionesCaracteristicasController(): base()
        { }

        [Authorize(Roles="LeerAplicacionesCaracteristicas")]
        public ActionResult Index()
        {
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("AplicacionesCaracteristicas");
        }

        [Authorize(Roles = "LeerAplicacionesCaracteristicas")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.AplicacionesCaracteristicasServicio();
            var registros = srv.Get()
                .Select(r => AplicacionCaracteristicaModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarAplicacionesCaracteristicas")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarAplicacionesCaracteristicas")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, AplicacionCaracteristicaModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.AplicacionesCaracteristicasServicio();
                    var modificar = srv.GetSingle(p => p.AplicacionCaracteristicaID == (long) model.AplicacionCaracteristicaID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        await srv.ApplyChangesAsync();
                        result = new[] { AplicacionCaracteristicaModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.AplicacionesCaracteristicas.ArtEntidad).Frase() };
                    }
                }
                catch (AplicacionCaracteristicaExisteException cee)
                {
                    log.Error($"Error al modificar {Txt.AplicacionesCaracteristicas.ArtEntidad}. Usuario: {CurrentUserID()}", cee);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + cee.Message, Txt.AplicacionesCaracteristicas.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error("Error al modificar el categoría con id=" + model.AplicacionCaracteristicaID, e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.AplicacionesCaracteristicas.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de características de aplicaciones.
        /// </summary>
        [Authorize(Roles = "LeerAplicacionesCaracteristicas")]
        public ActionResult ListaAplicacionesCaracteristicas()
        {
            IAplicacionesCaracteristicasServicio srv = Servicios.AplicacionesCaracteristicasServicio();
            IEnumerable<AplicacionCaracteristica> consulta = srv.Get();
            SelectList resul = new SelectList(consulta, "AplicacionCaracteristicaID", "Nombre");
            return Json(resul, JsonRequestBehavior.AllowGet);
        }
    }
}