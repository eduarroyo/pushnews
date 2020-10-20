using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.WebApp.Models.Aplicaciones;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Aplicaciones;
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
    public class AplicacionesController : BaseController
    {
        public AplicacionesController()
            : base()
        { }

        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return PartialView("Aplicaciones");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Leer([DataSourceRequest] DataSourceRequest request)
        {
            IAplicacionesServicio srv = Servicios.AplicacionesServicio();
            IEnumerable<Aplicacion> registros = srv.Get();
            IEnumerable<AplicacionGrid> resultado = registros.Select(r => AplicacionGrid.FromEntity(r));
            return Json(resultado.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult ExcelExportSave(string contentType, string base64, string fileName)
        {
            byte[] fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Modificar([DataSourceRequest] DataSourceRequest request, AplicacionGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    IAplicacionesServicio srv = Servicios.AplicacionesServicio();
                    Aplicacion modificar = srv.Get()
                        .SingleOrDefault(r => r.AplicacionID == model.AplicacionID);
                    if (modificar != null)
                    {
                        LoguearCambiosApiKey(modificar, model);
                        model.ActualizarEntidad(modificar);

                        ActualizarCaracteristicas(modificar, model.Caracteristicas);

                        await srv.ApplyChangesAsync();

                        if(Aplicacion.AplicacionID == modificar.AplicacionID)
                        {
                            Session["Aplicacion"] = modificar;
                            Session["Caracteristicas"] = modificar.Caracteristicas
                                .Where(c => c.Activo)
                                .Select(c => (Dominio.Enums.AplicacionCaracteristica)c.AplicacionCaracteristicaID);
                        }

                        result = new[] { AplicacionGrid.FromEntity(modificar) }.ToDataSourceResult(request, ModelState);
                    }
                    else
                    {
                        result.Errors = new[] { string.Format(Txt.ErroresComunes.NoExiste, Txt.Aplicaciones.ArtEntidad).Frase() };
                    }
                }
                catch (SubdominioExisteException see)
                {
                    log.Error($"Error al modificar {Txt.Aplicaciones.ArtEntidad} con id {model.AplicacionID}. Usuario: {CurrentUserID()}", see);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + see.Message, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
                catch (AplicacionExisteException see)
                {
                    log.Error($"Error al modificar {Txt.Aplicaciones.ArtEntidad} con id {model.AplicacionID}. Usuario: {CurrentUserID()}", see);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + see.Message, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error("Error al modificar " + Txt.Aplicaciones.ArtEntidad + " con id=" + model.AplicacionID, e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Nuevo([DataSourceRequest] DataSourceRequest request, AplicacionGrid model)
        {
            DataSourceResult result = new[] { model }.ToDataSourceResult(request, ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    IAplicacionesServicio srv = Servicios.AplicacionesServicio();
                    Aplicacion nueva = srv.Create();
                    model.ActualizarEntidad(nueva);
                    if(model.Caracteristicas != null && model.Caracteristicas.Any())
                    {
                        IAplicacionesCaracteristicasServicio acSrv = Servicios.AplicacionesCaracteristicasServicio();
                        nueva.Caracteristicas.AddRange(acSrv.Get(ca => model.Caracteristicas.Contains(ca.AplicacionCaracteristicaID)));
                    }
                    srv.Insert(nueva);
                    await srv.ApplyChangesAsync();
                    result = new[] { AplicacionGrid.FromEntity(nueva) }.ToDataSourceResult(request, ModelState);
                }
                catch (SubdominioExisteException see)
                {
                    log.Error($"Error al añadir {Txt.Aplicaciones.ArtEntidad}. Usuario: {CurrentUserID()}", see);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + see.Message, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
                catch (AplicacionExisteException see)
                {
                    log.Error($"Error al modificar {Txt.Aplicaciones.ArtEntidad}. Usuario: {CurrentUserID()}", see);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Modificar + see.Message, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
                catch (Exception e)
                {
                    log.Error("Error al añadir " + Txt.Aplicaciones.ArtEntidad + " " + model.Nombre, e);
                    result.Errors = new[] { string.Format(Txt.ErroresComunes.Aniadir, Txt.Aplicaciones.ArtEntidad).Frase() };
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Proporciona un modelo para lista de selección de aplicaciones.
        /// </summary>
        /// <param name="paisID">Si se especifica valor, se obtienen sólo las aplicaciones del país.
        /// </param>
        /// <param name="provinciaID">Si se especifica valor, se obtienen sólo las aplicaciones de
        /// la provincia.</param>
        [Authorize(Roles= "Administrador")]
        public ActionResult ListaAplicaciones()
        {
            IAplicacionesServicio srv = Servicios.AplicacionesServicio();
            IEnumerable<AplicacionGrid> consulta = srv.Get().Select(AplicacionGrid.FromEntity);
            return Json(consulta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[Authorize(Roles = "LeerAplicaciones")]
        public ActionResult EstablecerAplicacion(long aplicacionID, string urlActual)
        {
            Aplicacion aplicacionSeleccionada = Usuario.Aplicaciones.SingleOrDefault(cl => cl.AplicacionID == aplicacionID);
            if (aplicacionSeleccionada != null)
            {
                // Recuperar el mapa de redireccionamiento en caso de que volvamos a tener alguna url 
                // incompatible con el cambio de aplicación. Ver también la función cambiarAplicacion del
                // módulo pushnews.js
                //string nuevaUrl = MapaRedireccionamiento(urlActual);
                string mensaje = string.Format(Txt.Aplicaciones.AplicacionCambiada, aplicacionSeleccionada.Nombre);
                Session["Aplicacion"] = aplicacionSeleccionada;
                Session["Caracteristicas"] = aplicacionSeleccionada.Caracteristicas
                    .Where(c => c.Activo)
                    .Select(c => (Dominio.Enums.AplicacionCaracteristica)c.AplicacionCaracteristicaID);
                return Json(new
                {
                    resul = true,
                    titulo = Txt.Aplicaciones.CambiarAplicacionExito,
                    mensaje = mensaje
                });
            }
            else
            {
                return Json(new
                {
                    resul = false,
                    titulo = Txt.Aplicaciones.CambiarAplicacionError,
                    mensaje = Txt.Aplicaciones.NoExisteNoAsignada
                });
            }
        }


        //private string MapaRedireccionamiento(string urlOrigen)
        //{
        //    string cadenaAnalizar = urlOrigen;
        //    if (urlOrigen.Contains(value: '#'))
        //    {
        //        cadenaAnalizar = cadenaAnalizar.Substring(urlOrigen.IndexOf(value: '#'));
        //    }

        //    var reglas = new Dictionary<string, string>
        //    {
        //        //[ExpresionRegular] = "Expresion de salida",
        //        [@"^(?<base>#/.*)(?<resto>detalles.*)$"] = "${base}",
        //        [@"^(?<base>#/.*)(?!<resto>detalles.*)?$"] = "${base}",
        //    };

        //    foreach (var expresion in reglas.Keys)
        //    {
        //        var re = new Regex(expresion);
        //        Match matches = re.Match(cadenaAnalizar);
        //        if (matches.Success)
        //        {
        //            return re.Replace(cadenaAnalizar, reglas[expresion]);
        //        }
        //    }

        //    return "/";
        //}

        /// <summary>
        /// Actualiza la lista de características de una aplicación quitando y añadiendo las necesarias
        /// para que coincida con las indicadas en <paramref name="nuevaListaCaracteristicas"/>.
        /// </summary>
        /// <param name="aplicacion">Objeto correspondiente a la entidad Aplicación cuyas características van
        /// a ser modificadas.</param>
        /// <param name="nuevaListaCaracteristicas">IDs del nuevo conjunto de características de la
        /// aplicación.</param>
        private void ActualizarCaracteristicas(Aplicacion aplicacion, IEnumerable<long> nuevaListaCaracteristicas)
        {
            IAplicacionesCaracteristicasServicio srv = Servicios.AplicacionesCaracteristicasServicio();
            // Obtener las características que no se eliminan y las que se añaden como nuevas
            IEnumerable<long> caracteristicasActuales = aplicacion.Caracteristicas.Select(ca => ca.AplicacionCaracteristicaID);
            IEnumerable<long> caracteristicasAniadirIDs = nuevaListaCaracteristicas == null
                ? new long[0]
                : nuevaListaCaracteristicas.Where(caid => !caracteristicasActuales.Contains(caid));
            IEnumerable<AplicacionCaracteristica> caracteristicasNuevas =
                srv.Get(a => caracteristicasAniadirIDs.Contains(a.AplicacionCaracteristicaID));
            IEnumerable<AplicacionCaracteristica> caracteristicasMantener = 
                aplicacion.Caracteristicas
                .Where(aa => nuevaListaCaracteristicas.Contains(aa.AplicacionCaracteristicaID));

            // Limpiar las características actuales de la aplicación y establecer el conjunto unión de las
            // dos colecciones obtenidas antes.
            IEnumerable<AplicacionCaracteristica> caracteristicas = caracteristicasNuevas.Union(caracteristicasMantener);
            aplicacion.Caracteristicas.Clear();
            aplicacion.Caracteristicas.AddRange(caracteristicas);
        }

        private void LoguearCambiosApiKey(Aplicacion aplicacion, AplicacionGrid nuevosDatos)
        {
            if (aplicacion.ApiKey != nuevosDatos.ApiKey)
            {
                log.Info($"APIKEY CAMBIADA: Aplicación {aplicacion.Nombre} ({aplicacion.AplicacionID}) | ApiKey original: {aplicacion.ApiKey} | ApiKey nueva: {nuevosDatos.ApiKey}");
            }
        }
    }
}