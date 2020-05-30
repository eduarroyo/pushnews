using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Publicar
{
    public class PublicarAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Publicar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Publicar_default",
                "Publicar/{controller}/{action}/{id}",
                new { action = "Index", controller = "PublicarHome", id = UrlParameter.Optional }
            );
        }
    }
}