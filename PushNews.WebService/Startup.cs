using System;
using System.Linq;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(PushNews.WebService.Startup))]

namespace PushNews.WebService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}