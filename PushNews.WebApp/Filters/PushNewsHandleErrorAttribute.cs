using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Filters
{
    /// <summary>
    /// Clase que maneja el evento de errores globales. Si se produce una excepción en cualquier
    /// acción de cualquier controlador, se ejecutará el método OnException de esta clase, quedando
    /// registrado el error en el archivo de log sin cambiar el flujo de ejecución de otro modo.
    /// </summary>
    public class PushNewsHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILog log;

        public PushNewsHandleErrorAttribute()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var controller = (string)filterContext.RouteData.Values["controller"];
            var action = (string)filterContext.RouteData.Values["action"];

            string usuario; long usuarioId;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                usuario = filterContext.HttpContext.User.Identity.Name;
                usuarioId = filterContext.HttpContext.User.Identity.GetUserId<long>();
            }
            else
            {
                usuario = "Anónimo (no ha iniciado sesión)";
                usuarioId = 0;
            }

            string body = "";
            try
            {
                Stream s = filterContext.HttpContext.Request.InputStream;
                s.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
                {

                    body = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                body = "No se pudo leer el cuerpo de la solicitud: " + e.Message;
            }

            log.Error("Excepción en la acción \"" + action + "\" del controlador \"" + controller + "\"", filterContext.Exception);
            log.Error("Usuario: " + usuario + " - " + usuarioId + " | " + "Cuerpo de la solicitud: " + body);
            if (filterContext.Exception.GetType() == typeof(DbEntityValidationException))
            {
                DbEntityValidationException sqle = (DbEntityValidationException)filterContext.Exception;
                List<string> validationErrors = new List<string>();
                foreach (var entity in sqle.EntityValidationErrors)
                {
                    if (!entity.IsValid)
                    {
                        foreach (var eve in entity.ValidationErrors)
                        {
                            validationErrors.Add(eve.PropertyName + " -> " + eve.ErrorMessage);
                        }
                    }
                }
                log.Error("Error de validación de datos al guardar en la base de datos: \n\t ---->" + string.Join("\n\t ---->", validationErrors));
            }

            Exception inner = filterContext.Exception.InnerException;
            while (inner != null)
            {
                log.Error(
                 "Inner exception", inner);
                inner = inner.InnerException;
            }

            if (IsAjax(filterContext))
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new { Titulo = Txt.Comun.Error, Mensaje = filterContext.Exception.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}