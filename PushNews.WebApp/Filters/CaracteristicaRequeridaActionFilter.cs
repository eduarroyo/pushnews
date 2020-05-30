using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Filters
{
    public class CaracteristicasRequeridasAttribute : ActionFilterAttribute
    { 
        public string[] Caracteristicas { get; set; }
        private readonly ILog log;

        public CaracteristicasRequeridasAttribute(params string[] caracteristicas)
        {
            log = LogManager.GetLogger(this.GetType());
            Caracteristicas = caracteristicas;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Aplicacion aplicacion = (Aplicacion) HttpContext.Current.Session["Aplicacion"];

            if (aplicacion != null)
            {
                var dbcontext = HttpContext.Current.GetOwinContext().Get<IPushNewsUnitOfWork>();
                IAplicacionesServicio srvAplicaciones = new AplicacionesServicio(dbcontext);
                aplicacion = srvAplicaciones.GetSingle(a => a.AplicacionID == aplicacion.AplicacionID);
                string[] caracteristicasAplicacion = aplicacion.Caracteristicas.Select(c => c.Nombre).ToArray();
                if (Caracteristicas.All(c => caracteristicasAplicacion.Contains(c)))
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
            }

            log.WarnFormat(
                "El usuario {0} ha realizado una solicitud a {1} utilizando la aplicación \"{2}\" "
                + "que no tiene las características requeridas para usar dicha función: ({3}).",
                filterContext.HttpContext.User.Identity.Name, filterContext.HttpContext.Request.Url,
                aplicacion.Nombre, string.Join(", ", Caracteristicas));

            // Si no hay aplicación o la aplicación no tiene alguna de las características requeridas
            // se devuelve un error 403 Forbidden
            filterContext.Result = new JsonResult()
            {
                Data = new
                {
                    Titulo = Txt.ErroresComunes.FaltaCaracteristicaTitulo,
                    Mensaje = Txt.ErroresComunes.FaltaCaracteristicaMensaje
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}