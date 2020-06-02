using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PushNews.WebApp.Filters
{
    /// <summary>
    /// Filtro para establecer la aplicación de trabajo.
    /// </summary>
    public class SubdomainHandlerAttribute : ActionFilterAttribute
    {
        private readonly ILog log;

        public SubdomainHandlerAttribute()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Extraer el subdominio de la URL de la solicitud
            string host = filterContext.HttpContext.Request.Headers["Host"];
            Uri url = filterContext.HttpContext.Request.Url;
            string[] hostPartido = host.Split('.');

#if DEBUG
            #region Simular el subdominio utilizando el parámetro SubdominioDepuracion
            // Sólo en configuración de depuración: si el host no tiene subdominio (típico en
            // depuración) se inserta al principio del array hostPartido el subdominio de la aplicación de la
            // sesión, o en su defecto el subdominio por defecto almacenado en la tabla Parámetros de la base
            // de datos con la clave "SubdominioDepuracion".
            Aplicacion apl = (Aplicacion)HttpContext.Current.Session["Aplicacion"];
            if (apl != null)
            {
                hostPartido = new[] { apl.SubDominio, hostPartido[0] };
            }
            else if (hostPartido.Length < 2)
            {
                var dbcontext = HttpContext.Current.GetOwinContext().Get<IPushNewsUnitOfWork>();
                IParametrosServicio srvParametros = new ParametrosServicio(dbcontext/*, null*/);
                Parametro subdominioDepuracion = srvParametros.GetByName("SubdominioDepuracion");
                if (subdominioDepuracion != null)
                {
                    hostPartido = new[] { subdominioDepuracion.Valor, hostPartido[0] };
                }
            }
            #endregion
#endif

            // Si el host no contiene subdominio, redirigir a la página comercial.
            if (hostPartido.Length < 2)
            {
                log.Info($"Solicitud sin subdominio. Redirigir a la web comercial. Host de la solicitud: {host}");
                filterContext.Result = new RedirectResult("http://www.pushnews.com");
            }
            else
            {
                string subdominio = hostPartido[0];

                var dbcontext = HttpContext.Current.GetOwinContext().Get<IPushNewsUnitOfWork>();
                IParametrosServicio srvParametros = new ParametrosServicio(dbcontext/*, null*/);
                Parametro parametroSubdominioGenerico = srvParametros.GetByName("SubdominioGenerico");
                string subdominioGenerico = parametroSubdominioGenerico?.Valor ?? "";

                if (subdominio == subdominioGenerico)
                {
                    // Si se está accediendo con el subdominio genérico, no se puede dar acceso a la parte
                    // pública (webapp). Salvo que se solicite una url de la parte privada (backend) cuya
                    // ruta local empieza por "/backend", redirigirá a la web comercial.
                    if (!url.LocalPath.StartsWith("/backend"))
                    {
                        log.Info($"Solicitud a subdominio genérico fuera del backend. Redirigir a la web comercial. Url de la solicitud: {url.ToString()}");
                        filterContext.Result = new RedirectResult("http://www.pushnews.com");
                    }
                }
                else
                {
                    /* 14/3/2016 EAR
                     * Para garantizar que los cambios realizados en la versión, el tipo o el cloudkey de la 
                     * aplicación afecten a las sesiones abiertas, recargamos el objeto aplicación a la
                     * sesión en cada solicitud. Esto se puede quitar para producción si se quiere optimizar.
                     * eliminando los comentarios de la declaración de aplicacion (obtenida de la sesión) y
                     * del bloque condicional siguiente).
                     */

                    // Si hay un objeto Aplicacion almacenado en la sesión y el subdominio es el mismo, no
                    // hay que hacer nada más. Si por el contrario no hay objeto Aplicacion en la sesión, se 
                    // obtiene el objeto Aplicacion correspondiente al subdominio y se almacena en la sesíón.
                    Aplicacion aplicacion = (Aplicacion) HttpContext.Current.Session["Aplicacion"];
                    if (aplicacion == null)
                    {
                        // Obtener la aplicación asociada al subdominio
                        IAplicacionesServicio srvAplicaciones = new AplicacionesServicio(dbcontext);
                        aplicacion = srvAplicaciones.GetBySubdomain(subdominio);

                        // Si no se encuentra una aplicación asociada al subdominio, redirigir a la página comercial.
                        if (aplicacion == null)
                        {
                            log.Info($"El subdominio no corresponde a ninguna aplicación. Subdominio de la solicitud: {subdominio}");
                            filterContext.Result = new RedirectResult("http://www.pushnews.com");
                        }
                        else
                        {
                            // Guardar el objeto aplicación en la sesión
                            HttpContext.Current.Session.Add("Aplicacion", aplicacion);
                            HttpContext.Current.Session.Add("Caracteristicas", aplicacion.Caracteristicas
                                .Where(c => c.Activo)
                                .Select(c => (Dominio.Enums.AplicacionCaracteristica)c.AplicacionCaracteristicaID));
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}