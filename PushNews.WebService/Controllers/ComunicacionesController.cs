﻿using PushNews.Dominio.Entidades;
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
            // Validación del modelo: Subdominio se ha especificado.
            if (string.IsNullOrWhiteSpace(model.Subdominio))
            {
                return BadRequest("Subdominio no válido");
            }
            if(!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();

            // Si categoriaID es -1, nos están pidiendo las publicaciones destacadas, sin filtrar por
            // categoría.
            IEnumerable<Comunicacion> comunicaciones = model.CategoriaID == -1
                ? await srv.PublicadasAsync(categoriaID: null, soloDestacadas: true, timestamp: model.TimeStamp)
                : await srv.PublicadasAsync(categoriaID: model.CategoriaID, timestamp: model.TimeStamp);

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
                Aplicacion app = Aplicacion(model.Subdominio);
                IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
                Comunicacion c = srv.ConsultarComunicacion(model.ComunicacionID.Value, model.UID, GetClientIp());
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