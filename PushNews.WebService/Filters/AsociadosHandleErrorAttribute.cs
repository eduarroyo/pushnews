using log4net;
using System.Linq;
using System.Web.Http.Filters;

namespace PushNews.WebService.Filters
{
    /// <summary>
    /// Clase que maneja el evento de errores globales. Si se produce una excepción en cualquier
    /// acción de cualquier controlador, se ejecutará el método OnException de esta clase, quedando
    /// registrado el error en el archivo de log sin cambiar el flujo de ejecución de otro modo.
    /// </summary>
    public class AsociadosHandleErrorAttribute : ExceptionFilterAttribute
    {
        private readonly ILog log;

        public AsociadosHandleErrorAttribute()
        {
            log = LogManager.GetLogger("Asociados");
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            string controller = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string action = context.ActionContext.ActionDescriptor.ActionName;
            string[] parametros = context.ActionContext.ActionArguments.Select(arg => $"{arg.Key}: {arg.Value}").ToArray();
            string strParametros = "{" + string.Join(", ", parametros) + "}" ;
            
            log.Error(
                $"Excepción en la acción \"{action}\" del controlador \"{controller}\" con los parámetros {strParametros}",
                context.Exception);
        }
    }
}