using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using PushNews.WebService.Services;
using PushNews.Dominio.Entidades;
using Newtonsoft.Json;
using log4net;

namespace PushNews.WebService.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string publicClientId;
        private ILog log;

        public ApplicationOAuthProvider(string publicClientId)
        {
            log = LogManager.GetLogger("General");
            if (publicClientId == null)
            {
                throw new ArgumentNullException(paramName: "publicClientId");
            }

            this.publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var form = await context.Request.ReadFormAsync();
            string subdominio = form["subdominio"] ?? "";
            string apiKey = form["apiKey"] ?? "";

            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password) || string.IsNullOrEmpty(subdominio) || string.IsNullOrEmpty(apiKey))
            {
                context.SetError(
                    error: "bad_request",
                    errorDescription: "Faltan parámetros de acceso username, password, subdominio o apikey");
                return;
            }

            AsociadosUserManager userManager = context.OwinContext.GetUserManager<AsociadosUserManager>();
            Asociado user = await userManager.FindAsync(context.UserName, context.Password, subdominio, apiKey);

            if (user == null)
            {
                log.Info($"Intento de login con credenciales no válidas: {{usuario: {context.UserName}, clave: {context.Password}, subdominio: {subdominio}, apiKey:{apiKey}}}");

                context.SetError(
                    error: "invalid_grant",
                    errorDescription: "El nombre de usuario, la contraseña, el subdominio o la clave de la api no son correctos.");
                return;
            }

            if(!user.Activo)
            {
                log.Info($"Intento de login de asociado desactivado: {{usuario: {context.UserName}, clave: {context.Password}, subdominio: {subdominio}, apiKey:{apiKey}}}");
                context.SetError(
                    error: "invalid_grant",
                    errorDescription: "El usuario ha sido desactivado.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager);

            AuthenticationProperties properties = CreateProperties(user.UserName, user);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(result: null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // La credenciales de la contraseña del propietario del recurso no proporcionan un id. de cliente.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(result: null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, relativeUri: "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(result: null);
        }

        public static AuthenticationProperties CreateProperties(string userName, Asociado asociado)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "usuario", JsonConvert.SerializeObject(Models.ModificarAsociadoModel.FromEntity(asociado)) },

            };
            return new AuthenticationProperties(data);
        }
    }
}