using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNews.WebService.Controllers
{
    public class ComunicacionesController : BaseController
    {
        [Route("comunicaciones/lista")]
        [HttpPost]
        public async Task<IHttpActionResult> Lista(SolicitudComunicacionesModel model)
        {
            if(!ComprobarClaves(model))
            {
                return Unauthorized();
            }
            // Validación del modelo: Subdominio se ha especificado.
            if (string.IsNullOrWhiteSpace(model.Subdominio))
            {
                return BadRequest("Subdominio no válido");
            }

            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();

            // Si categoriaID es -1, nos están pidiendo las publicaciones destacadas, sin filtrar por
            // categoría.
            IEnumerable<Comunicacion> comunicaciones = model.CategoriaID == -1
                ? await srv.PublicadasAsync(categoriaID: null, soloDestacadas: true, timestamp: model.TimeStamp, incluirPrivadas: User.Identity.IsAuthenticated)
                : await srv.PublicadasAsync(categoriaID: model.CategoriaID, timestamp: model.TimeStamp, incluirPrivadas: User.Identity.IsAuthenticated);

            return Ok(comunicaciones
                        .Select(ComunicacionDetalleModel.FromEntity)
                        .AsQueryable());
        }

        [Route("comunicaciones/detalle")]
        [HttpPost]
        public async Task<IHttpActionResult> Detalle(SolicitudComunicacionesModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            // Validación del modelo: ComunicacionID tiene valor, UID se ha especificado, Subdominio se ha especificado.
            if (!model.ComunicacionID.HasValue || string.IsNullOrWhiteSpace(model.UID) || string.IsNullOrWhiteSpace(model.Subdominio))
            {
                log.Warn($"Modelo no válido: ComunicacionID={model.ComunicacionID}, UID={model.UID}, subdominio={model.Subdominio}.");
                return BadRequest();
            }

            ComunicacionDetalleModel resul;

            try
            {
                long? asociadoId = null;
                Aplicacion app = Aplicacion(model.Subdominio);
                bool aplicacionTieneAsociados = app.Caracteristicas.Any(ac => ac.Nombre == "Asociados");
                if (aplicacionTieneAsociados && User.Identity.IsAuthenticated)
                {
                    asociadoId = RequestContext.Principal.Identity.GetUserId<long>();
                }

                IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
                Comunicacion c = srv.ConsultarComunicacion(model.ComunicacionID.Value, model.UID, GetClientIp(), asociadoId);
                await srv.ApplyChangesAsync();
                resul = ComunicacionDetalleModel.FromEntity(c);
                return Ok(resul);
            }
            catch (Exception e)
            {
                log.Error($"Error al consultar comunicación {model.ComunicacionID} el terminal {model.UID}.", e);
                return Ok<ComunicacionDetalleModel>(null);
            }

        }
    }
}