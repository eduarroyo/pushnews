using PushNews.WebApp.Areas.Backend.Models;
using PushNews.WebApp.Controllers;
using PushNews.WebApp.Models.Comunicaciones;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class EscritorioController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            double[] coordenadas = CoordenadasPorDefecto();
            ViewBag.Latitud = coordenadas[0];
            ViewBag.Longitud = coordenadas[1];
            return PartialView("Escritorio");
        }

        [Authorize]
        public ActionResult Estadisticas()
        {
            EstadisticasModel resul = new EstadisticasModel();

            ITerminalesServicio trmSrv = Servicios.TerminalesServicio();
            resul.Terminales = trmSrv.Get().Count();

            IComunicacionesServicio comSrv = Servicios.ComunicacionesServicio();
            resul.Comunicaciones = comSrv.Get().Count();

            IComunicacionesAccesosServicio accSrv = Servicios.ComunicacionesAccesosServicio();
            DateTime haceUnMes = DateTime.Now.AddMonths(-1);
            resul.VisualizacionesUltimoMes = accSrv.Get(acc => acc.Fecha >= haceUnMes).Count();
            resul.Visualizaciones = accSrv.Get().Count();
            
            return Json(resul, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult UltimasComunicaciones()
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            IEnumerable<Comunicacion> ultimas1 = srv
                .Get(c => !c.Borrado, cc => cc.OrderByDescending(c => c.FechaCreacion),
                    includeProperties: "Categoria, Adjunto, Accesos"
                )
                .Take(10);

            List<ComunicacionGrid> ultimas = new List<ComunicacionGrid>();
            var categoriasPermitidas = Usuario.Categorias.Select(c => c.CategoriaID);
            foreach(var c in ultimas1)
            {
                ultimas.Add(ComunicacionGrid.FromEntity(c, categoriasPermitidas, PeriodoEnvioPushHoras));
            }

            return Json(ultimas, JsonRequestBehavior.AllowGet);
        }
    }
}