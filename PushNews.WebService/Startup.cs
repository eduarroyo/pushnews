using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PushNews.WebService.Startup))]

namespace PushNews.WebService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        { }
    }
}