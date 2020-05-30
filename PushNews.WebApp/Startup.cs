using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(PushNews.WebApp.Startup))]

namespace PushNews.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}