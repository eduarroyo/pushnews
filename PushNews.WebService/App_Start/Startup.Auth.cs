using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.WebService.Providers;
using PushNews.WebService.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PushNews.WebService
{
    public partial class Startup
    {
        // Habilite la aplicación para que use OAuthAuthorization. A continuación, puede proteger sus API web
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(value: "/Token"),
                AuthorizeEndpointPath = new PathString(value: "/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(value: 14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // Para obtener más información para configurar la autenticación, visite http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure el contexto de base de datos y el administrador de usuarios y el 
            // administrador de roles para usar una única instancia por solicitud
            app.CreatePerOwinContext<IPushNewsUnitOfWork>(CreateModel);
            app.CreatePerOwinContext<AsociadosUserManager>(AsociadosUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Permitir que la aplicación use una cookie para almacenar información para el usuario que inicia sesión
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString(value: "/Publicar/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<AsociadosUserManager, Asociado, long>(
            //            validateInterval: TimeSpan.FromMinutes(value: 20),
            //            regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
            //            getUserIdCallback: (claim) => int.Parse(claim.GetUserId()))
            //    }
            //});

            // Permitir que la aplicación use tokens portadores para autenticar usuarios
            app.UseOAuthBearerTokens(OAuthOptions);
        }

        public static Dictionary<string, string> HostsCS = new Dictionary<string, string>();
        
        /// <summary>
        /// Genera el contexto de la base de datos.
        /// </summary>
        public PushNews.Dominio.IPushNewsUnitOfWork CreateModel()
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["PushNewsModel"];
            var model = new PushNewsModel(cs.ConnectionString);
            return model;
        }
    }
}