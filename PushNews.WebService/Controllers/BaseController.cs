﻿using System.Linq;
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
using System.Collections.Generic;
using PushNews.WebService.Models;

namespace PushNews.WebService.Controllers
{
    public abstract class BaseController : ApiController
    {
        private IServiciosFactoria servicios;
        protected readonly ILog log;
        private Aplicacion aplicacion;
        private string subdominio;

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

        private IPushNewsUnitOfWork DataContext
        {
            get
            {
                return Request.GetOwinContext().Get<IPushNewsUnitOfWork>();
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
            base.Dispose(disposing);
        }

        protected string GetClientIp()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        protected virtual bool ComprobarClaves(SolicitudModel model)
        {
            Aplicacion ap = Aplicacion(model.Subdominio);

            // Para autorizar la solicitud debe cumplirse que la ApiKey coincida con la de la aplicación
            // y que, si la aplicación requiere clave de suscripción, la clave coincida también.
            // Tanto en la ApiKey guardada en la aplicación como en la recibida en el modelo, se considera
            // que el valor nulo es igual que el de cadena vacía.
            return (ap.ApiKey ?? "") == (model.ApiKey ?? "");
        }
    }
}