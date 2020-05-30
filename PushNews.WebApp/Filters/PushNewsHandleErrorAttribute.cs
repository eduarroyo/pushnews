using System.Web.Mvc;
using log4net;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Filters
{
    /// <summary>
    /// Clase que maneja el evento de errores globales. Si se produce una excepción en cualquier
    /// acción de cualquier controlador, se ejecutará el método OnException de esta clase, quedando
    /// registrado el error en el archivo de log sin cambiar el flujo de ejecución de otro modo.
    /// </summary>
    public class PushNewsHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILog log;

        public PushNewsHandleErrorAttribute()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public override void OnException(ExceptionContext filterContext)
        {
            bool ajax = IsAjax(filterContext);
            bool customErrorsEnabled = filterContext.HttpContext.IsCustomErrorEnabled;
            string controller = (string)filterContext.RouteData.Values["controller"];
            string action = (string)filterContext.RouteData.Values["action"];

            log.Error(
                $"Excepción en la acción \"{action}\" del controlador \"{controller}\". " +
                (ajax ? "Es una solicitud ajax. " : "No es una solicitud ajax. ") +
                (customErrorsEnabled ? "Errores personalizados habilitados. " : "Errores personalizados deshabilitados. ") +
                $"La excepción {(filterContext.ExceptionHandled ? " ya " : " NO ")} ha sido manejada.",
                filterContext.Exception);

            if (filterContext.ExceptionHandled || !customErrorsEnabled)
            {
                return;
            }
            
            if (ajax)
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new { Titulo = Txt.Comun.Error, Mensaje = filterContext.Exception.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                //filterContext.HttpContext.Response.Clear();
            }
            else
            {
                base.OnException(filterContext);
            }

        }
    }
}