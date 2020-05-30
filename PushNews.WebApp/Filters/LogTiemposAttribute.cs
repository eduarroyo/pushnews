using log4net;
using System.Web.Mvc;

namespace PushNews.WebApp.Filters
{
    public class LogTiemposAttribute : ActionFilterAttribute
    { 
        private readonly ILog log;

        public LogTiemposAttribute()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug("ActionExecuted: " + filterContext.HttpContext.Request.Url);
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug("ActionExecuting: " + filterContext.HttpContext.Request.Url);
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Debug("ResultExecuting: " + filterContext.HttpContext.Request.Url);
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Debug("ResultExecuted: " + filterContext.HttpContext.Request.Url);
            base.OnResultExecuted(filterContext);
        }
    }
}