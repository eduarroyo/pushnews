using PushNews.WebApp.Filters;
using System.Web.Mvc;

namespace PushNews.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SubdomainHandlerAttribute());
            filters.Add(new PushNewsHandleErrorAttribute());
        }
    }
}
