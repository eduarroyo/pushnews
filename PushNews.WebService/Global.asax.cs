using PushNews.WebService.Filters;
using System.Web.Http;

namespace PushNews.WebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Registrar el filtro para forzar HTTPS.
            //GlobalConfiguration.Configuration.Filters.Add(new RequireHttpsAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new PushNewsHandleErrorAttribute());

            // Establecer configuración del logger
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
