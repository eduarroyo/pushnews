using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using PushNews.WebService.Models;
using PushNews.WebService.Models.Requests;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Device.Location;

namespace PushNews.WebService.Controllers
{
    public class HermandadesController : BaseController
    {
        [Route("hermandades"), HttpPost]
        public IHttpActionResult Hermandades(SolicitudModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            var ap = Aplicacion(model.Subdominio);
            if(ap.Caracteristicas.All(c => c.Nombre != "Hermandades"))
            {
                return Unauthorized();
            }

            IHermandadesServicio srv = Servicios.HermandadesServicio();
            IEnumerable<HermandadModel> hermandades = srv.Get(h => h.Activo)
                                                         .Select(HermandadModel.FromEntity);
            return Ok(hermandades);
        }

        [Route("inforutas"), HttpPost]
        public IHttpActionResult RutasHermandades(SolicitudRutasActivasModel model)
        {
            if (!ComprobarClaves(model))
            {
                return Unauthorized();
            }

            var ap = Aplicacion(model.Subdominio);
            if (ap.Caracteristicas.All(c => c.Nombre != "Hermandades"))
            {
                return Unauthorized();
            }

            bool filtroProximidad;
            // Validar los campos del modelo de la solicitud.
            if(model.Latitud.HasValue && model.Latitud >= -90 && model.Latitud <= 90 && 
               model.Longitud.HasValue && model.Longitud >= -180 && model.Longitud <= 180 &&
               model.Distancia > 0)
            {
                // Válido con filtro por proximidad
                filtroProximidad = true;
            }
            else if(!model.Latitud.HasValue && !model.Longitud.HasValue && model.Distancia == 0)
            {
                // Válido sin filtro por proximidad
                filtroProximidad = false;
            }
            else
            {
                // Solicitud no válida
                return BadRequest();
            }

            RutasActivasModel respuesta = new RutasActivasModel();
            IEmpresasServicio eSrv = Servicios.EmpresasServicio();

            // No enviamos los patrocinadores que no tengan coordenadas porque no se van a poder
            // representar en el mapa.
            respuesta.Patrocinadores = eSrv.Get(p => p.Longitud.HasValue && p.Latitud.HasValue)
                                           .Select(EmpresaModel.FromEntity)
                                           .AsQueryable();

            // No enviamos las hermandades que no tengan coordenadas porque no se van a poder
            // representar en el mapa.
            IHermandadesServicio hSrv = Servicios.HermandadesServicio();
            respuesta.Hermandades = hSrv.Get(h => h.Activo && h.IglesiaLongitud.HasValue && h.IglesiaLatitud.HasValue)
                                        .Select(HermandadModel.FromEntity)
                                        .AsQueryable();
            
            IRutasServicio rSrv = Servicios.RutasServicio();
            respuesta.Rutas = rSrv.RutasActivas(Aplicacion(model.Subdominio).AplicacionID)
                                  .Select(RutaModel.FromEntity)
                                  .AsQueryable();

            // Si los parámetros de la solicitud indican que hay que filtrar por distancia, aplicar
            // los filtros a los patrocinadores y a las hermandades que tengan coordenadas.
            if(filtroProximidad)
            {
                respuesta.Patrocinadores = respuesta.Patrocinadores
                    .Where(rp => Distancia(rp.Latitud.Value, rp.Longitud.Value,
                                           model.Latitud.Value, model.Longitud.Value)
                                <= model.Distancia)
                    .ToList();

                respuesta.Hermandades = respuesta.Hermandades
                    .Where(rp => Distancia(rp.IglesiaLatitud.Value, rp.IglesiaLongitud.Value,
                                           model.Latitud.Value, model.Longitud.Value)
                                <= model.Distancia)
                    .ToList();

                // Las rutas no estoy pasándolas por el filtro de distancia porque sería más complicado
                // debido a la cantidad de puntos.
            }

            return Ok(respuesta);
        }

        public double Distancia(double latitud1, double longitud1, double latitud2, double longitud2)
        {
            GeoCoordinate c1 = new GeoCoordinate(latitud1, longitud1);
            GeoCoordinate c2 = new GeoCoordinate(latitud2, longitud2);
            double distancia = c1.GetDistanceTo(c2);
            return distancia;
        }
    }
}