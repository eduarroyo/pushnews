using PushNews.WebApp.Helpers;
using PushNews.WebApp.Services;
using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PushNews.WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        private IServiciosFactoria servicios;
        private ApplicationUserManager userManager;
        private IFileManager fileManager;
        protected readonly ILog log;
        private Aplicacion aplicacion;

        public BaseController()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        protected IServiciosFactoria Servicios
        {
            get
            {
                if (servicios == null)
                {
                    servicios = new ServiciosFactoria(DataContext, Aplicacion);
                }
                return servicios;
            }
        }

        protected IFileManager FileManager
        {
            get
            {
                if (fileManager == null)
                {
                    // Se usa LocalFileManager en depuración y cuando se publica en la máquina virtual de Azure
                    fileManager = new LocalFileManager(Servicios);
                    // Se usa AzureFileManager cuando se publica como aplicación Web de Azure.
                    //fileManager = new AzureFileManager(servicios);
                }
                return fileManager;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                if (userManager == null)
                {
                    userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return userManager;
            }
        }

        private EmailService emailService;
        protected EmailService EmailService
        {
            get
            {
                if (emailService == null)
                {
                    emailService = new EmailService();
                }
                return emailService;
            }
        }

        public IPushNewsUnitOfWork DataContext
        {
            get
            {
                return HttpContext.Request.GetOwinContext().Get<IPushNewsUnitOfWork>();
            }
        }
        
        /// <summary>
        /// Obtien la aplicación de la sesión para facilitar el acceso desde los controladores.
        /// </summary>
        protected Aplicacion Aplicacion
        {
            get
            {
                if (aplicacion == null)
                {
                    aplicacion = (Aplicacion) Session["Aplicacion"];
                }
                return aplicacion;
            }
        }

        private Usuario usuario = null;
        protected Usuario Usuario
        {
            get
            {
                if (usuario == null && User.Identity.IsAuthenticated)
                {
                    long usuarioID = long.Parse(User.Identity.GetUserId());
                    usuario = UserManager.FindById(usuarioID);
                }
                return usuario;
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0] // obtain it from HTTP header AcceptLanguages
                    : null;
            }

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                userManager = null;
            }
            base.Dispose(disposing);
        }

        protected double[] CoordenadasPorDefecto()
        {
            IParametrosServicio srv = Servicios.ParametrosServicio();
            double[] coordenadas = { 0D, 0D }; // lat, lng
            if (Aplicacion != null)
            {
                Parametro pCoordenadas = srv.GetSingle(p => p.AplicacionID == Aplicacion.AplicacionID
                                                        && p.Nombre == "CoordenadasIniciales");

                // Si no hay parámetro o falla al parsear, devolverá [0,0].
                if (pCoordenadas != null)
                {
                    Regex er = new Regex(@"^(?<latitud>[-+]?(?:\d*[.])?\d+);(?<longitud>[-+]?(?:\d*[.])?\d+)$");
                    Match m = er.Match(pCoordenadas.Valor);
                    if (m.Success)
                    {
                        coordenadas[0] = double.Parse(m.Groups["latitud"].Value, CultureInfo.InvariantCulture);
                        coordenadas[1] = double.Parse(m.Groups["longitud"].Value, CultureInfo.InvariantCulture);

                        if (coordenadas[0] > 90 || coordenadas[0] < -90 || coordenadas[1] > 180 || coordenadas[1] < -180)
                        {
                            log.Error($"Valor de parámetro CoordenadasIniciales de la aplicación {Aplicacion.AplicacionID} FUERA DE RANGO.");
                            coordenadas[0] = 0;
                            coordenadas[1] = 0;
                        }
                    }
                    else
                    {
                        log.Error($"Error de formato en el parámetro CoordenadasIniciales de la aplicación {Aplicacion.AplicacionID}");
                    }
                }
            }
            return coordenadas;
        }

        private double? _horas = null;
        protected double PeriodoEnvioPushHoras
        {
            get
            {
                if (!_horas.HasValue) {
                    IParametrosServicio pSrv = Servicios.ParametrosServicio();
                    Parametro horasAtras = pSrv.GetByName("HorasEnvioPush");
                    _horas = 1;
                    if (horasAtras != null)
                    {
                        double aux = 1;
                        if (double.TryParse(horasAtras.Valor, out aux))
                        {
                            _horas = aux;
                        }
                    }
                }

                return _horas ?? 1;
            }
        }

        /// <summary>
        /// Indica si una excepción de actualización de la base de datos está provocada por una violación de
        /// clave foránea.
        /// </summary>
        protected bool ExceptionIsForeignKeyViolation(DbUpdateException dbue)
        {
            SqlException sqle = dbue.GetBaseException() as SqlException;
            return sqle != null && sqle.Errors.Count > 0 && sqle.Errors[0].Number == 547;
        }


        public string RenderToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string ObtenerPlantillaUrlMapas()
        {
            IParametrosServicio pSrv = Servicios.ParametrosServicio();
            Parametro pUrlMapas = pSrv.GetByName("UrlGoogleMaps");
            return pUrlMapas?.Valor
                ?? "http://maps.google.com/maps?z=12&t=m&q=loc:{latitud}+{longitud}";
        }
    }
}