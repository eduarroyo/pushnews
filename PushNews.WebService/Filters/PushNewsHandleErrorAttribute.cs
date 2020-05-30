using log4net;
using System.Linq;
using System.Web.Http.Filters;

namespace PushNews.WebService.Filters
{
    internal class PushNewsHandleErrorAttribute : ExceptionFilterAttribute
    {
        private readonly ILog log;

        public PushNewsHandleErrorAttribute()
        {
            log = LogManager.GetLogger("General");
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            string controller = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string action = context.ActionContext.ActionDescriptor.ActionName;
            string[] parametros = context.ActionContext.ActionArguments.Select(arg => $"{arg.Key}: {arg.Value}").ToArray();
            string strParametros = "{" + string.Join(", ", parametros) + "}";

            log.Error(
                $"Excepción en la acción \"{action}\" del controlador \"{controller}\" con los parámetros {strParametros}",
                context.Exception);
        }
    }
}