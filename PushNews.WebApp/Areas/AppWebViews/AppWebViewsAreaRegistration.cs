using System.Web.Mvc;

namespace PushNews.WebApp.Areas.AppWebViews
{
    public class AppWebViewsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AppWebViews";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AppWebViews_default",
                "AppWebViews/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}