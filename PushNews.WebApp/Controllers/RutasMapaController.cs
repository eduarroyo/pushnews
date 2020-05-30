using PushNews.WebApp.Models.Empresas;
using PushNews.WebApp.Models.Hermandades;
using PushNews.WebApp.Models.RutasMapa;
using PushNews.Dominio.Entidades;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PushNews.WebApp.Controllers
{
    public class RutasMapaController : BaseController
    {
        // GET: Rutas
        public ActionResult GMaps(string ps = "", bool op = false)
        {
            log.Debug("Solicitud de mapa GMaps;" +
                ";UrlReferrer: " + Request.UrlReferrer +
                ";UserAgent: " + Request.UserAgent +
                ";UserHostName: " + Request.UserHostName +
                ";UserIP:" + Request.UserHostAddress +
                ";Headers: " + Request.Headers.ToString());

            if(!ValidarRequest(ps))
            {
                log.Info("RutasMapa: Soliciud de web de mapa bloqueada");
                return RedirectToAction("DescargaApps");
            }

            // Filtro por defecto.
            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pFechaDomingoRamos = pSrv.GetByName("FechaDomingoRamos");
            DateTime fechaDomingoRamos;
            string tag = "";
            if (pFechaDomingoRamos != null && DateTime.TryParse(pFechaDomingoRamos.Valor, out fechaDomingoRamos))
            {
                DateTime fechaCiclo;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime hoyCET = TimeZoneInfo.ConvertTime(DateTime.Now.Date, tzi); 
                string[] tags = { "TODAS_DOMINGORAMOS", "TODAS_LUNESSANTO",   "TODAS_MARTESSANTO", "TODAS_MIERCOLESSANTO",
                                  "TODAS_JUEVESSANTO",  "TODAS_VIERNESSANTO", "TODAS", "TODAS_DOMINGORESURRECCION" };

                for (int i = 0; i <= 7; i++)
                {
                    fechaCiclo = fechaDomingoRamos.AddDays(i);
                    log.Debug("Hoy CET: " + hoyCET.ToShortDateString() + " Hoy servidor: " + fechaCiclo.ToShortDateString());

                    // Probando diferentes formas de comparación. Parece que ni con == ni con Compare hace la comparación
                    // que quiero en el servidor. Quizás es por la diferencia de la zona horaria.
                    if (hoyCET.Year == fechaCiclo.Year && hoyCET.Month == fechaCiclo.Month && hoyCET.Day == fechaCiclo.Day)
                    {
                        tag = tags[i];
                        break;
                    }
                }
            }
            ViewBag.TagFiltro = tag;

            // En función del valor op, indicamos a la vista si debe mostrar o no los banners.
            Parametro pHabilitarPublicidadEnMapa = pSrv.GetByName("HabilitarPublicidadEnMapa");
            string strHabilitarPublicidadEnMapa = pHabilitarPublicidadEnMapa?.Valor ?? "false";
            bool habilitarPublicidadEnMapa = strHabilitarPublicidadEnMapa == "true";
            ViewBag.BannersHabilitados = !op && habilitarPublicidadEnMapa;

            log.Debug("Filtro por defecto por tag " + tag);
            return View("RutasMapa");
        }

        public ActionResult MapBox(string ps, bool op = false)
        {
            log.Debug("Solicitud de mapa MAPBOX;" +
                ";UrlReferrer: " + Request.UrlReferrer +
                ";UserAgent: " + Request.UserAgent +
                ";UserHostName: " + Request.UserHostName +
                ";UserIP:" + Request.UserHostAddress +
                ";Headers: " + Request.Headers.ToString());

            if (!ValidarRequest(ps))
            {
                log.Info("RutasMapa: Soliciud de web de mapa bloqueada");
                return RedirectToAction("DescargaApps");
            }

            // Filtro por defecto.
            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pFechaDomingoRamos = pSrv.GetByName("FechaDomingoRamos");
            DateTime fechaDomingoRamos;
            string tag = "";
            if (pFechaDomingoRamos != null && DateTime.TryParse(pFechaDomingoRamos.Valor, out fechaDomingoRamos))
            {
                DateTime fechaCiclo;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime hoyCET = TimeZoneInfo.ConvertTime(DateTime.Now.Date, tzi);
                string[] tags = { "TODAS_DOMINGORAMOS", "TODAS_LUNESSANTO",   "TODAS_MARTESSANTO", "TODAS_MIERCOLESSANTO",
                                  "TODAS_JUEVESSANTO",  "TODAS_VIERNESSANTO", "TODAS", "TODAS_DOMINGORESURRECCION" };

                for (int i = 0; i <= 7; i++)
                {
                    fechaCiclo = fechaDomingoRamos.AddDays(i);
                    log.Debug("Hoy CET: " + hoyCET.ToShortDateString() + " Hoy servidor: " + fechaCiclo.ToShortDateString());

                    // Probando diferentes formas de comparación. Parece que ni con == ni con Compare hace la comparación
                    // que quiero en el servidor. Quizás es por la diferencia de la zona horaria.
                    if (hoyCET.Year == fechaCiclo.Year && hoyCET.Month == fechaCiclo.Month && hoyCET.Day == fechaCiclo.Day)
                    {
                        tag = tags[i];
                        break;
                    }
                }
            }
            ViewBag.TagFiltro = tag;

            // En función del valor op, indicamos a la vista si debe mostrar o no los banners.
            Parametro pHabilitarPublicidadEnMapa = pSrv.GetByName("HabilitarPublicidadEnMapa");
            string strHabilitarPublicidadEnMapa = pHabilitarPublicidadEnMapa?.Valor ?? "false";
            bool habilitarPublicidadEnMapa = strHabilitarPublicidadEnMapa == "true";
            ViewBag.BannersHabilitados = !op && habilitarPublicidadEnMapa;

            log.Debug("Filtro por defecto por tag " + tag);
            return View("RutasMapaMapBox");
        }

        public ActionResult Index(string ps, bool op = false)
        {
            log.Debug("|Solicitud de mapa|OpenStreetMap|" +
                "|" + Request.UrlReferrer +
                "|" + Request.UserAgent +
                "|" + Request.UserHostName +
                "|" + Request.UserHostAddress +
                "|" + Request.Headers.ToString());

            if (!ValidarRequest(ps))
            {
                log.Info("RutasMapa: Soliciud de web de mapa bloqueada");
                return RedirectToAction("DescargaApps");
            }

            // Filtro por defecto.
            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pFechaDomingoRamos = pSrv.GetByName("FechaDomingoRamos");
            DateTime fechaDomingoRamos;
            string tag = "";
            if (pFechaDomingoRamos != null && DateTime.TryParse(pFechaDomingoRamos.Valor, out fechaDomingoRamos))
            {
                DateTime fechaCiclo;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime hoyCET = TimeZoneInfo.ConvertTime(DateTime.Now.Date, tzi);
                string[] tags = { "TODAS_DOMINGORAMOS", "TODAS_LUNESSANTO",   "TODAS_MARTESSANTO", "TODAS_MIERCOLESSANTO",
                                  "TODAS_JUEVESSANTO",  "TODAS_VIERNESSANTO", "TODAS", "TODAS_DOMINGORESURRECCION" };

                for (int i = 0; i <= 7; i++)
                {
                    fechaCiclo = fechaDomingoRamos.AddDays(i);
                    log.Debug("Hoy CET: " + hoyCET.ToShortDateString() + " Hoy servidor: " + fechaCiclo.ToShortDateString());

                    // Probando diferentes formas de comparación. Parece que ni con == ni con Compare hace la comparación
                    // que quiero en el servidor. Quizás es por la diferencia de la zona horaria.
                    if (hoyCET.Year == fechaCiclo.Year && hoyCET.Month == fechaCiclo.Month && hoyCET.Day == fechaCiclo.Day)
                    {
                        tag = tags[i];
                        break;
                    }
                }
            }
            ViewBag.TagFiltro = tag;

            // En función del valor op, indicamos a la vista si debe mostrar o no los banners.
            Parametro pHabilitarPublicidadEnMapa = pSrv.GetByName("HabilitarPublicidadEnMapa");
            string strHabilitarPublicidadEnMapa = pHabilitarPublicidadEnMapa?.Valor ?? "false";
            bool habilitarPublicidadEnMapa = strHabilitarPublicidadEnMapa == "true";
            ViewBag.BannersHabilitados = !op && habilitarPublicidadEnMapa;

            log.Debug("Filtro por defecto por tag " + tag);
            return View("RutasMapaOSM");
        }

        public ActionResult TomTom(string ps, bool op = false)
        {
            log.Debug("|Solicitud de mapa|TOMTOM|" +
                "|" + Request.UrlReferrer +
                "|" + Request.UserAgent +
                "|" + Request.UserHostName +
                "|" + Request.UserHostAddress +
                "|" + Request.Headers.ToString());

            if (!ValidarRequest(ps))
            {
                log.Info("RutasMapa: Soliciud de web de mapa bloqueada");
                return RedirectToAction("DescargaApps");
            }

            // Filtro por defecto.
            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pFechaDomingoRamos = pSrv.GetByName("FechaDomingoRamos");
            DateTime fechaDomingoRamos;
            string tag = "";
            if (pFechaDomingoRamos != null && DateTime.TryParse(pFechaDomingoRamos.Valor, out fechaDomingoRamos))
            {
                DateTime fechaCiclo;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime hoyCET = TimeZoneInfo.ConvertTime(DateTime.Now.Date, tzi);
                string[] tags = { "TODAS_DOMINGORAMOS", "TODAS_LUNESSANTO",   "TODAS_MARTESSANTO", "TODAS_MIERCOLESSANTO",
                                  "TODAS_JUEVESSANTO",  "TODAS_VIERNESSANTO", "TODAS", "TODAS_DOMINGORESURRECCION" };

                for (int i = 0; i <= 7; i++)
                {
                    fechaCiclo = fechaDomingoRamos.AddDays(i);
                    log.Debug("Hoy CET: " + hoyCET.ToShortDateString() + " Hoy servidor: " + fechaCiclo.ToShortDateString());

                    // Probando diferentes formas de comparación. Parece que ni con == ni con Compare hace la comparación
                    // que quiero en el servidor. Quizás es por la diferencia de la zona horaria.
                    if (hoyCET.Year == fechaCiclo.Year && hoyCET.Month == fechaCiclo.Month && hoyCET.Day == fechaCiclo.Day)
                    {
                        tag = tags[i];
                        break;
                    }
                }
            }
            ViewBag.TagFiltro = tag;

            // En función del valor op, indicamos a la vista si debe mostrar o no los banners.
            Parametro pHabilitarPublicidadEnMapa = pSrv.GetByName("HabilitarPublicidadEnMapa");
            string strHabilitarPublicidadEnMapa = pHabilitarPublicidadEnMapa?.Valor ?? "false";
            bool habilitarPublicidadEnMapa = strHabilitarPublicidadEnMapa == "true";
            ViewBag.BannersHabilitados = !op && habilitarPublicidadEnMapa;

            log.Debug("Filtro por defecto por tag " + tag);
            return View("RutasMapaTomTom");
        }

        [HttpPost]
        public ActionResult RutasHermandades(RutasHermandadesRequest model, string ps = "")
        {
            log.Debug("Solicitud de datos de hermandades: " +
                " UrlReferrer: " + Request.UrlReferrer +
                " UserAgent: " + Request.UserAgent +
                " UserHostName: " + Request.UserHostName +
                " UserIP:" + Request.UserHostAddress +
                " Headers: " + Request.Headers.ToString());

            if (!ValidarRequest(ps))
            {
                log.Info("RutasMapa: Soliciud de datos bloqueada");
                return Json(new { redirectUrl = Url.Action("DescargaApps" ,"RutasMapa") });
            }

            string subdominio;
#if DEBUG
            subdominio = "hermandadesdecordoba";
#else
            subdominio = Request.Url.Host.Split('.')[0];
#endif
            IAplicacionesServicio appSrv = new AplicacionesServicio(DataContext);
            Aplicacion app = appSrv.GetBySubdomain(subdominio);
            if(app == null)
            {
                return Json($"No se encontró la aplicación: subdominio {subdominio} incorrecto");
            }

            bool filtroProximidad;
            // Validar los campos del modelo de la solicitud.
            if (model.Latitud.HasValue && model.Latitud >= -90 && model.Latitud <= 90 &&
               model.Longitud.HasValue && model.Longitud >= -180 && model.Longitud <= 180 &&
               model.Distancia > 0)
            {
                // Válido con filtro por proximidad
                filtroProximidad = true;
            }
            else if (!model.Latitud.HasValue && !model.Longitud.HasValue && model.Distancia == 0)
            {
                // Válido sin filtro por proximidad
                filtroProximidad = false;
            }
            else
            {
                // Solicitud no válida
                return Json(false);
            }

            IEmpresasServicio eSrv = new EmpresasServicio(DataContext, app);

            RutasActivasModel respuesta = new RutasActivasModel();
            // No enviamos los patrocinadores que no tengan coordenadas porque no se van a poder
            // representar en el mapa.
            Random rnd = new Random();
            respuesta.Patrocinadores = eSrv.Get(p => p.Activo /*&& p.Longitud.HasValue && p.Latitud.HasValue*/)
                                           .Select(EmpresaModel.FromEntity)
                                           .AsQueryable();

            // No enviamos las hermandades que no tengan coordenadas porque no se van a poder
            // representar en el mapa.
            IHermandadesServicio hSrv = Servicios.HermandadesServicio();
            respuesta.Hermandades = hSrv.Get(h => h.Activo && h.IglesiaLongitud.HasValue && h.IglesiaLatitud.HasValue)
                                        .Select(HermandadModel.FromEntity)
                                        .AsQueryable();

            //Cargar las rutas activas y sus posiciones
            IRutasServicio rSrv = Servicios.RutasServicio();
            IQueryable<Ruta> rutasActivas = rSrv.RutasActivas(app.AplicacionID).AsQueryable();
            List<RutaModel> resultadoRutas = new List<RutaModel>();

            IParametrosServicio pSrv = new ParametrosServicio(DataContext);
            Parametro pMaxPuntosRuta = pSrv.GetByName("MaximoPuntosRuta");
            int maxPuntosRuta = int.Parse(pMaxPuntosRuta?.Valor ?? "10");
            Parametro pDistanciaCorteRuta = pSrv.GetByName("DistanciaCorteRuta");
            int distanciaCorteRuta = int.Parse(pDistanciaCorteRuta?.Valor ?? "20");

            IGpsPosicionesServicio gpSrv = new GpsPosicionesServicio(DataContext);

            foreach(Ruta r in rutasActivas)
            {
                RutaModel rm = RutaModel.FromEntity(r);
                rm.Posiciones = gpSrv.PosicionesCabezaRuta(r, maxPuntosRuta, distanciaCorteRuta)
                    .Select(PosicionModel.FromEntity)
                    .ToList();
                resultadoRutas.Add(rm);
            }
            respuesta.Rutas = resultadoRutas;

            // Si los parámetros de la solicitud indican que hay que filtrar por distancia, aplicar
            // los filtros a los patrocinadores y a las hermandades que tengan coordenadas.
            if (filtroProximidad)
            {
                respuesta.Patrocinadores = respuesta.Patrocinadores
                    .Where(rp => Util.Distancia(rp.Latitud.Value, rp.Longitud.Value,
                                           model.Latitud.Value, model.Longitud.Value)
                                <= model.Distancia)
                    .ToList();

                respuesta.Hermandades = respuesta.Hermandades
                    .Where(rp => Util.Distancia(rp.IglesiaLatitud.Value, rp.IglesiaLongitud.Value,
                                           model.Latitud.Value, model.Longitud.Value)
                                <= model.Distancia)
                    .ToList();

                // Las rutas no estoy pasándolas por el filtro de distancia.
            }


            respuesta.Patrocinadores = respuesta.Patrocinadores.OrderBy(i => rnd.Next()).ToList();

            return Json(respuesta);
        }
        

        [NonAction]
        private bool ValidarRequest(string parametroSeguridad)
        {
            IParametrosServicio pSrv = Servicios.ParametrosServicio();

            Parametro pSeguridad = pSrv.GetByName("ParametrosSeguridad");
            string pseg = pSeguridad?.Valor ?? "";
            List<string> ParametrosSeguridad = pseg.Split(';', ',', ' ').ToList();
            
            // el parámetro es válido si no se han especificado códigos de seguridad
            // o si la lista de códigos de seguridad contiene el parámetro.
            if (ParametrosSeguridad.Any() && !ParametrosSeguridad.Contains(parametroSeguridad))
            {
                log.Info("Solicitud bloqueada por parámetro no permitido: " + parametroSeguridad);
                return false;
            }

            Parametro pBloqueados = pSrv.GetByName("OrigenesBloqueados");
            string bloqueados = pBloqueados?.Valor ?? "";
            List<string> Bloqueados = bloqueados.Split(';', ' ', ',').Where(b => !string.IsNullOrEmpty(b)).ToList();

            // El origen de la solicitud se considera bloqueado si, teniendo valor 
            // Request.UrlReferrer, dicho valor contiene alguno de los fragmentos de la lista de
            // Bloqueados.
            string referrer = Request.UrlReferrer?.ToString();
            if(!string.IsNullOrEmpty(referrer) && Bloqueados.Any(b => referrer.Contains(b)))
            {
                log.Info("Solicitud bloqueada porque el origen está bloqueado: " + Request.UrlReferrer);
                return false;
            }

            return true;
        }

        public ActionResult DescargaApps()
        {
            ViewBag.UrlPlayStore = Aplicacion.PlayStoreUrl;
            ViewBag.UrlITunes = Aplicacion.ITunesUrl;
            return View("DescargaApps");
        }
    }
}