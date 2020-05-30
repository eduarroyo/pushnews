using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebService.Filters;
using PushNews.WebService.Models.ApiClientesExternos;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    [AsociadosHandleError]
    public class AsociadosController : BaseController
    {
        private ILog logAsociados = LogManager.GetLogger("Asociados");

        [HttpPost, Route("asociados/lista")]
        public async Task<IHttpActionResult> Lista(SolicitudAsociadosModel model)
        {
            try
            {
                logAsociados.Info($"Solicitud lista asociados: {JsonConvert.SerializeObject(model)}");
                if (ComprobarClavesAsociados(model))
                {
                    logAsociados.Info("ApiKey válido.");
                    IAsociadosServicio srv = Servicios.AsociadosServicio();
                    IEnumerable<Asociado> asociados = await srv.GetAsync();
                    return Ok(asociados.Select(AsociadoResponseModel.FromEntity));
                }
                else
                {
                    logAsociados.Info("ApiKey no válido.");
                    return Unauthorized();
                }
            }
            catch
            {
                logAsociados.Error("Error al obtener la lista de asociados.");
                throw;
            }
        }

        [HttpPost, Route("asociados/nuevo")]
        public async Task<IHttpActionResult> Nuevo(CrearAsociadoModel model)
        {
            try
            {
                logAsociados.Info($"Solicitud nuevo asociado: {JsonConvert.SerializeObject(model)}");
                if (ComprobarClavesAsociados(model))
                {
                    logAsociados.Info("ApiKey válido.");
                    if (ModelState.IsValid)
                    {
                        logAsociados.Info("Datos válidos.");
                        Asociado nuevo = Servicios.AsociadosServicio().Create();
                        model.ActualizarEntidad(nuevo);
                        nuevo.AplicacionID = Aplicacion(model.Subdominio).AplicacionID;
                        var result = await UserManager.CreateAsync(nuevo, model.Clave);
                        if (result.Succeeded)
                        {
                            logAsociados.Info("Asociado creado con éxito.");
                            // Pongo true en el body para asegurar que, en un posible cliente web,
                            // se disparen todos los eventos al recibir la respuesta.
                            return Ok(true);
                        }
                        else
                        {
                            string errores = string.Join(" ## ", result.Errors);
                            logAsociados.Info($"Error al crear asociado: {errores}");
                            return BadRequest($"Error al crear el asociado: {errores}");
                        }
                    }
                    else
                    {
                        logAsociados.Info($"Datos no válidos: {JsonConvert.SerializeObject(ModelState.Values)}");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    logAsociados.Info("ApiKey no válido.");
                    return Unauthorized();
                }
            }
            catch(Exception e)
            {
                logAsociados.Error($"Error al crear asociado.", e);
                throw;
            }
        }

        [HttpPost, Route("asociados/modificar")]
        public async Task<IHttpActionResult> Modificar(ModificarAsociadoModel model)
        {
            try
            {
                logAsociados.Info($"Solicitud modificar asociado: {JsonConvert.SerializeObject(model)}");
                if (ComprobarClavesAsociados(model))
                {
                    logAsociados.Info("ApiKey válido.");
                    if (ModelState.IsValid)
                    {
                        logAsociados.Info("Datos válidos.");
                        Asociado modificar = await UserManager.FindByIdAsync(model.AsociadoID, Aplicacion(model.Subdominio).AplicacionID);
                        if(modificar != null)
                        {
                            model.ActualizarEntidad(modificar);
                            var result = await UserManager.UpdateAsync(modificar);
                            if (result.Succeeded)
                            {
                                logAsociados.Info("Asociado modificado.");
                                // Pongo true en el body para asegurar que, en un posible cliente web,
                                // se disparen todos los eventos al recibir la respuesta.
                                return Ok(true);
                            }
                            else
                            {
                                logAsociados.Info($"Error al modificar asociado: {string.Join(", ", result.Errors)}");
                                return BadRequest(string.Join(", ", result.Errors));
                            }
                        }
                        else
                        {
                            logAsociados.Info($"Error al modificar asociado: el asociado solicitado {model.AsociadoID} no existe o no pertenece a la aplicación del subdominio {model.Subdominio}.");
                            return BadRequest("Asociado no encontrado");
                        }
                    }
                    else
                    {
                        logAsociados.Info($"Datos no válidos: {JsonConvert.SerializeObject(ModelState.Values)}");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    logAsociados.Info("ApiKey no válido.");
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                logAsociados.Error($"Error al modificar asociado.", e);
                throw;
            }
        }

        [HttpPost, Route("asociados/eliminar")]
        public async Task<IHttpActionResult> Eliminar(EliminarAsociadoModel model)
        {
            try
            {
                logAsociados.Info($"Solicitud eliminar asociado: {JsonConvert.SerializeObject(model)}");
                if (ComprobarClavesAsociados(model))
                {
                    logAsociados.Info("ApiKey válido.");
                    if (ModelState.IsValid)
                    {
                        logAsociados.Info("Datos válidos.");
                        Asociado eliminar = await UserManager.FindByIdAsync(model.AsociadoID);
                        if (eliminar != null)
                        {
                            var result = await UserManager.DeleteAsync(eliminar);
                            if (result.Succeeded)
                            {
                                logAsociados.Info("Asociado eliminado.");
                                return Ok();
                            }
                            else
                            {
                                logAsociados.Info($"Error al eliminar asociado: {string.Join(" ## ", result.Errors)}");
                                return Content(System.Net.HttpStatusCode.InternalServerError, "Error al eliminar el asociado");
                            }
                        }
                        else
                        {
                            logAsociados.Info($"No existe un asociado con id={model.AsociadoID}.");
                            return BadRequest("No existe un asociado con ese id.");
                        }
                    }
                    else
                    {
                        logAsociados.Info($"Datos no válidos: {JsonConvert.SerializeObject(ModelState.Values)}");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    logAsociados.Info("ApiKey no válido.");
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                logAsociados.Error($"Error al eliminar asociado.", e);
                throw;
            }
        }
    }
}