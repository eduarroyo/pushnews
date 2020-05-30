using System.Linq;
using System.Web;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using System.Web.Http;
using System.Net.Http;
using PushNews.WebService.Services;
using System.Collections.Generic;
using PushNews.WebService.Models;
using PushNews.WebService.Models.ApiClientesExternos;

namespace PushNews.WebService.Controllers
{
    public abstract class BaseController : ApiController
    {
        private IServiciosFactoria servicios;
        protected readonly ILog log;
        private Aplicacion aplicacion;
        private string subdominio;
        private AsociadosUserManager userManager;

        public BaseController()
        {
            log = LogManager.GetLogger("General");
        }

        protected IServiciosFactoria Servicios
        {
            get
            {
                if (servicios == null)
                {
                    // Pasar la clínica a la factoría para que incialice cada servicio con la
                    // opción de la clínica para utilizar clientes, productos, etc. globales o no.
                    servicios = new ServiciosFactoria(DataContext, aplicacion);
                }
                return servicios;
            }
        }

        protected AsociadosUserManager UserManager
        {
            get
            {
                if (userManager == null)
                {
                    userManager = Request.GetOwinContext().GetUserManager<AsociadosUserManager>();
                }
                return userManager;
            }
        }
        
        private IPushNewsUnitOfWork DataContext
        {
            get
            {
                return Request.GetOwinContext().Get<IPushNewsUnitOfWork>();
            }
        }
        
        private Asociado usuario = null;
        protected Asociado Usuario
        {
            get
            {
                if (usuario == null && User.Identity.IsAuthenticated)
                {
                    long usuarioID = long.Parse(User.Identity.GetUserId());
                    usuario = UserManager.Users.Single(u => u.AsociadoID == usuarioID);
                }
                return usuario;
            }
        }

        protected long CurrentUserID()
        {
            if(User.Identity.IsAuthenticated)
            {
                return long.Parse(User.Identity.GetUserId());
            }
            else
            {
                return 0;
            }
        }

        protected Aplicacion Aplicacion(string subdominio)
        {
            // EAR 24/1/2016
            // Este método es UNA ADAPTACIÓN CHAPUCERA de la versión anterior.
            // Antes tomaba el subdominio de la lista de parámetros de la URL porque siempre era GET. Ahora 
            // no sé la forma de acceder al cuerpo de la solicitud (POST) para obtener el subdominio 
            // automáticametne. Hay que tener mucho cuidado porque la propiedad privada aplicación de la 
            // clase base será null aunque se haya especificado el subdominio en los parámetros. El valor se
            // asigna la primera vez que se invoca a este método.

            if (aplicacion == null || subdominio != aplicacion.SubDominio)
            {
                this.subdominio = subdominio;
                IDictionary<string, string> dicconarionParametros = Request.GetQueryNameValuePairs().ToDictionary(k => k.Key, v => v.Value);
                aplicacion = DataContext.Aplicaciones.SingleOrDefault(a => a.SubDominio == subdominio);
            }
            return aplicacion;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                userManager = null;
            }
            base.Dispose(disposing);
        }

        protected string GetClientIp()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
        
        /// <summary>
        /// Valida una solicitud para la sección de asociados. Para que la solcitud sea válida, debe aportar
        /// la ApiKey de clientes externos correspondiente a la aplicación asociada al subdominio de la
        /// solicitud, y dicha aplicación debe tener activa la característica "Asociados".
        /// </summary>
        protected bool ComprobarClavesAsociados(SolicitudAsociadosModel model)
        {
            Aplicacion ap = Aplicacion(model.Subdominio);

            // Para autorizar la solicitud debe cumplirse que la ApiKey coincida con la de la aplicación
            // y que, si la aplicación requiere clave de suscripción, la clave coincida también.
            // Tanto en la ApiKey guardada en la aplicación como en la recibida en el modelo, se considera
            // que el valor nulo es igual que el de cadena vacía.
            return ap.PermitirAccesoApiExternos && (ap.ApiKeyExternos ?? "") == (model.ApiKey ?? "");
        }

        protected virtual bool ComprobarClaves(SolicitudModel model)
        {
            Aplicacion ap = Aplicacion(model.Subdominio);

            // Para autorizar la solicitud debe cumplirse que la ApiKey coincida con la de la aplicación
            // y que, si la aplicación requiere clave de suscripción, la clave coincida también.
            // Tanto en la ApiKey guardada en la aplicación como en la recibida en el modelo, se considera
            // que el valor nulo es igual que el de cadena vacía.
            return (ap.ApiKey ?? "") == (model.ApiKey ?? "")
                && (!ap.RequerirClaveSuscripcion || model.Clave == ap.ClaveSuscripcion);
        }
    }
}