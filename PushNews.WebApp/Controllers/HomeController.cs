using PushNews.WebApp.Filters;
using PushNews.WebApp.Models;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(long? id = null)
        {
            if(id.HasValue)
            {
                var comSrv = Servicios.ComunicacionesServicio();
                Comunicacion com = comSrv.GetSingle(c => c.ComunicacionID == id.Value);
                ViewBag.Comunicacion = ComunicacionDetalleModel.FromEntity(com);
            }

            var srv = Servicios.CategoriasServicio();
            var registros = srv.ListaCategorias()
                               .Select(CategoriaModel.FromEntity)
                               .ToList();

            registros.Insert(0,
                new CategoriaModel
                {
                    CategoriaID = 0,
                    Nombre = Txt.Comunicaciones.TodasLasComunicaciones,
                    Orden = -1,
                    Activo = true,
                    Icono = "fa-asterisk"
                });

            registros.Insert(1,
                new CategoriaModel
                {
                    CategoriaID = -1,
                    Nombre = Txt.Comunicaciones.Destacados,
                    Orden = 0,
                    Activo = true,
                    Icono = "fa-star"
                });

            ViewBag.Categorias = registros;
            return View();
        }
        
        /// <summary>
        /// Devuelve la lista de publicaciones de la aplicación actual para la web, aplicando posibles
        /// restricciones de filtrado, ordenación y paginado.
        /// </summary>
        /// <returns>DataSourceResult para un componente telerik tipo Grid, ListView o similar.</returns>
        public async Task<ActionResult> ComunicacionesPublicadas([DataSourceRequest] DataSourceRequest request)
        {
            // Si el único filtro es CategoriaID == -1, tenemos que devolver sólo las comunicaciones destacadas
            if (request.Filters.Count == 1)
            {
                FilterDescriptor filtro = request.Filters[0] as FilterDescriptor;
                if(filtro != null
                    && filtro.Member == "CategoriaID"
                    && filtro.Operator == FilterOperator.IsEqualTo 
                    && filtro.Value.ToString() == "-1")
                {
                    request.Filters.Clear();
                    request.Filters.Add(new FilterDescriptor("Destacado", FilterOperator.IsEqualTo, true));
                }
            }

            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            IEnumerable<ComunicacionModel> comunicacionesPublicadas =
                Aplicacion == null 
                ? new List<ComunicacionModel>(0)
                : (await srv.PublicadasAsync()).Select(ComunicacionModel.FromEntity);

            return Json(comunicacionesPublicadas.ToDataSourceResult(request),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve una comunicación de la aplicación actual dado su ID.
        /// </summary>
        /// <param name="comunicacionID">ID de la comunicación solicitada.</param>
        public async Task<ActionResult> ComunicacionDetalle(long comunicacionID)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion c = srv.ConsultarComunicacion(comunicacionID, $"{Request.UserHostName}-{Request.Browser.Browser}", Request.UserHostAddress);
            await srv.ApplyChangesAsync();
            ComunicacionDetalleModel resul = ComunicacionDetalleModel.FromEntity(c);
            return Json(resul, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Documento(long id, bool descargar = false)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion comunicacion = srv.GetSingle(c => c.ComunicacionID == id);

            if (comunicacion == null || comunicacion.Adjunto == null)
            {
                return HttpNotFound();
            }

            var cd = new ContentDisposition
            {
                FileName = comunicacion.Adjunto.Nombre,
                Inline = !descargar,
            };

            Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
            Response.AppendHeader(name: "Content-Length", value: comunicacion.Adjunto.Tamano.ToString());
            FileStream str = FileManager.ObtenerDocumento(comunicacion.Adjunto);
            return new FileStreamResult(str, comunicacion.Adjunto.Mime);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Imagen(long id)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion comunicacion = srv.GetSingle(c => c.ComunicacionID == id);

            if (comunicacion != null)
            {
                if (comunicacion.Imagen == null)
                {
                    return File("~/Content/Images/Blanco.png", contentType: "image/png");
                }
                else
                {
                    var cd = new ContentDisposition
                    {
                        FileName = comunicacion.Imagen.Nombre,
                        Inline = false,
                    };

                    Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                    FileStream str = FileManager.ObtenerDocumento(comunicacion.Imagen);
                    return new FileStreamResult(str, comunicacion.Imagen.Mime);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Miniatura(long id)
        {
            IComunicacionesServicio srv = Servicios.ComunicacionesServicio();
            Comunicacion comunicacion = srv.GetSingle(c => c.ComunicacionID == id);

            if (comunicacion != null)
            {
                if (comunicacion.Imagen == null)
                {
                    return File("~/Content/Images/Blanco.png", contentType: "image/png");
                }
                else
                {
                    var cd = new ContentDisposition
                    {
                        FileName = Path.GetFileNameWithoutExtension(comunicacion.Imagen.Nombre) + "_thumb"
                                    + Path.GetExtension(comunicacion.Imagen.Nombre),
                        Inline = false,
                    };
                    
                    Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                    FileStream str = FileManager.ObtenerMiniatura(comunicacion.Imagen);
                    return new FileStreamResult(str, comunicacion.Imagen.Mime);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Logotipo(long? id = null)
        {
            IAplicacionesServicio srv = Servicios.AplicacionesServicio();
            long appID = id ?? Aplicacion.AplicacionID;
            Aplicacion aplicacion = srv.GetSingle(c => c.AplicacionID == appID);

            if (aplicacion != null)
            {
                if (aplicacion.Logotipo == null)
                {
                    return File("~/Content/Images/PushNewsLogo.png", contentType: "image/png");
                }
                else
                {

                    var cd = new ContentDisposition
                    {
                        FileName = aplicacion.Logotipo.Nombre,
                        Inline = false,
                    };
                    Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                    FileStream str = FileManager.ObtenerDocumento(aplicacion.Logotipo);
                    return new FileStreamResult(str, aplicacion.Logotipo.Mime);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult LogotipoEmpresa(long id)
        {
            IEmpresasServicio srv = Servicios.EmpresasServicio();
            Empresa empresa = srv.GetSingle(e => e.EmpresaID == id);

            if (empresa != null)
            {
                if (empresa.Logotipo == null)
                {
                    return File("~/Content/Images/Blanco.png", contentType: "image/png");
                }
                else
                {
                    var cd = new ContentDisposition
                    {
                        FileName = empresa.Logotipo.Nombre,
                        Inline = false,
                    };
                    Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                    FileStream str = FileManager.ObtenerDocumento(empresa.Logotipo);
                    return new FileStreamResult(str, empresa.Logotipo.Mime);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult BannerEmpresa(long id)
        {
            IEmpresasServicio srv = Servicios.EmpresasServicio();
            Empresa empresa = srv.GetSingle(e => e.EmpresaID == id && e.Activo);
            
            FileStream str;
            string mime;
            string fileName;
            if (empresa == null || empresa.Banner == null)
            {
                return File("~/Content/Images/Blanco.png", "image/png");
            }
            else
            {
                str = FileManager.ObtenerDocumento(empresa.Banner);
                mime = empresa.Banner.Mime;
                fileName = empresa.Banner.Nombre;

                var cd = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false,
                };

                Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                return new FileStreamResult(str, mime);
            }            
        }

        public ActionResult ListaCategorias()
        {
            var srv = Servicios.CategoriasServicio();
            var registros = srv.Get(c => c.Activo, cc => cc.OrderBy(c => c.Orden))
                               .Select(CategoriaModel.FromEntity)
                               .ToList();
            return Json(registros, JsonRequestBehavior.AllowGet);
        }
    }
}