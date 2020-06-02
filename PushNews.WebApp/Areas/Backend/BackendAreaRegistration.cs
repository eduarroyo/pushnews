using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend
{
    public class BackendAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Backend";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Backend_default",
                "Backend/{controller}/{action}/{id}",
                new { controller= "BackendHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}