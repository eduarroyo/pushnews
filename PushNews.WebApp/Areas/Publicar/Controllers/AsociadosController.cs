using PushNews.WebApp.Controllers;
using PushNews.WebApp.Filters;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Account;
using PushNews.WebApp.Models.Asociados;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Asociados;
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

namespace PushNews.WebApp.Areas.Publicar.Controllers
{
    [Authorize][CaracteristicasRequeridas("Asociados")]
    public class AsociadosController : BaseController
    {
        public AsociadosController(): base()
        { }

        [Authorize(Roles="LeerAsociados")]
        public ActionResult Index()
        {
            ViewBag.TipoAplicacion = Aplicacion.Tipo;
            return PartialView("Asociados");
        }

        [Authorize(Roles = "LeerAsociados")]
        public async Task<ActionResult> Leer([DataSourceRequest] DataSourceRequest request)
        {
            var srv = Servicios.AsociadosServicio();
            var registros = (await srv.GetAsync())
                .Select(r => AsociadoModel.FromEntity(r))
                .ToList();
            return Json(registros.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "LeerAsociados")]
        public ActionResult LeerEstadisticas([DataSourceRequest] DataSourceRequest request, long comunicacionID)
        {
            // Obtener todos los accesos a la consulta donde hay valor para asociados.
            // En principio todos los accesos a consultas deberían tener un asociado, pero podría
            // darse el caso en el futuro de aplicaciones a las que se ha añadido la característica
            // asociados tiempo después de haber estado funcionando o de aplicaciones con 
            // comunicaciones públicas y privadas (sólo para asociados).
            IComunicacionesAccesosServicio caSrv = Servicios.ComunicacionesAccesosServicio();
            IEnumerable<ComunicacionAcceso> accesos = caSrv
                .Get(ca => ca.ComunicacionID == comunicacionID && ca.AsociadoID.HasValue,
                     includeProperties: "Asociado");

            IEnumerable<IGrouping<Asociado, ComunicacionAcceso>> bbb = accesos.GroupBy(a => a.Asociado);    
    
            IEnumerable<EstadisticasAsociadoModel> resultado = bbb
                .Select(ga => new EstadisticasAsociadoModel
                {
                    ComunicacionID = comunicacionID,
                    AsociadoID = ga.Key.AsociadoID,
                    UltimaConsultaFecha = ga.Max(aa => aa.Fecha),
                    TotalConsultas = ga.Count(),
                    AsociadoNombre = ga.Key.Nombre + " " + ga.Key.Apellidos
                });

            return Json(resultado.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "ExportarAsociados")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = "EliminarAsociados")]
        public async Task<ActionResult> Eliminar([DataSourceRequest] DataSourceRequest request, AsociadoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            try
            {
                Asociado eliminar = await AsociadosManager.FindByIdAsync(model.AsociadoID);
                if (eliminar != null)
                {
                    var resultado = await AsociadosManager.DeleteAsync(eliminar);
                    if(resultado.Succeeded)
                    {
                        log.Info($"Asociado {eliminar.Codigo} eliminado desde el backend.");
                    }
                    else
                    {
                        log.Debug($"Eliminar: Fallo al eliminar {Txt.Asociados.ArtEntidad}. {string.Join(", ", resultado.Errors)}");
                        result.Errors = resultado.Errors;
                    }
                }
                else
                {
                    log.Debug($"Eliminar: {Txt.Asociados.ArtEntidad} con id={model.AsociadoID} no existe.");
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Asociados.ArtEntidad).Frase() };
                }
            }
            catch (Exception e)
            {
                log.Error($"Error al eliminar {Txt.Asociados.ArtEntidad} con id={model.AsociadoID}", e);
                result.Errors = new[] { string.Format(Txt.ErroresComunes.Eliminar, Txt.Asociados.ArtEntidad).Frase() };
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarAsociados")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, AsociadoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.AsociadosServicio();
                    var modificar = await AsociadosManager.FindByIdAsync(model.AsociadoID, Aplicacion.AplicacionID);
                    if (modificar != null)
                    {
                        model.ActualizarEntidad(modificar);
                        var resultado = await AsociadosManager.UpdateAsync(modificar);
                        if (resultado.Succeeded)
                        {
                            log.Info($"Asociado {modificar.Codigo} modificado desde el backend.");
                            result = new[] { AsociadoModel.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                        }
                        else
                        {
                            log.Info($"Error al modificar desde el backend {Txt.Asociados.ArtEntidad} con código {modificar.Codigo}. {string.Join(", ", resultado.Errors)}");
                            result.Errors = resultado.Errors;
                        }
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Asociados.ArtEntidad).Frase() };
                    }
                }
                catch (AsociadoExisteException cee)
                {
                    log.Error($"Error al modificar {Txt.Asociados.ArtEntidad}. Usuario: {CurrentUserID()}", cee);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + cee.Message, Txt.Asociados.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error($"Error al modificar {Txt.Asociados.ArtEntidad} con id={model.AsociadoID}.", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Asociados.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "ModificarAsociados")]
        public async Task<ActionResult> CambiarClave(CambiarClaveUsuarioModel model)
        {
            var result = new { errors = new List<string>() };
            if (ModelState.IsValid)
            {
                Asociado asociado = await AsociadosManager.FindByIdAsync(model.UsuarioID);
                if (asociado != null)
                {
                    IdentityResult ir = await AsociadosManager.ChangePasswordAsync(model.UsuarioID, model.Clave);
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
                    result.errors.Add(string.Format(Txt.ErroresComunes.NoExiste, Txt.Asociados.ArtEntidad).Frase());
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
        [Authorize(Roles = "CrearAsociados")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, AsociadoModel model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    var srv = Servicios.AsociadosServicio();
                    var nuevo = srv.Create();
                    model.ActualizarEntidad(nuevo);
                    nuevo.AplicacionID = Aplicacion.AplicacionID;

                    var resultado = await AsociadosManager.CreateAsync(nuevo, model.Clave);
                    if (resultado.Succeeded)
                    {
                        log.Info($"Asociado {nuevo.Codigo} creado con éxito desde el backend.");
                        result = new[] { AsociadoModel.FromEntity(nuevo) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        log.Info($"Error al crear {Txt.Asociados.ArtEntidad} {nuevo.Codigo} desde el backend: {string.Join(", ", result.Errors)}");
                        result.Errors = resultado.Errors;
                    }
                }
                catch (AsociadoExisteException cee)
                {
                    log.Error($"Error al añadir {Txt.Asociados.ArtEntidad}. Usuario: {CurrentUserID()}", cee);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + cee.Message, Txt.Asociados.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error($"Error al añadir {Txt.Asociados.ArtEntidad} {model.Nombre}.", e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Asociados.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de motivos de baja.
        /// </summary>
        [Authorize(Roles = "LeerAsociados")]
        public ActionResult ListaAsociados()
        {
            var srv = Servicios.AsociadosServicio();
            var registros = srv.Get(c => c.Activo).Select(AsociadoModel.FromEntity);
            return Json(registros, JsonRequestBehavior.AllowGet);
        }
    }
}