using PushNews.WebApp.Controllers;
using PushNews.WebApp.Helpers;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PushNews.WebApp.Areas.Backend.Controllers
{
    [Authorize]
    public class AdjuntosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> NuevoDocumento(HttpPostedFileBase DocumentoAdjunto)
        {
            Documento nuevo = null;
            if (DocumentoAdjunto != null)
            {
                nuevo = await FileManager.NuevoDocumento(DocumentoAdjunto);
            }

            object resul = null;
            if (nuevo != null)
            {
                resul = new
                {
                    DocumentoID = nuevo.DocumentoID,
                    Nombre = nuevo.Nombre,
                    Url = Rutas.UrlAdjuntoSinEnlazar(nuevo.DocumentoID)
                };
            }
            return Json(resul);
        }

        [HttpPost]
        public async Task<ActionResult> NuevaImagen(HttpPostedFileBase ImagenAdjunta)
        {
            Documento nuevo = null;
            if (ImagenAdjunta != null)
            {
                nuevo = await FileManager.NuevaImagen(ImagenAdjunta);
            }

            object resul = null;
            if (nuevo != null)
            {
                resul = new
                {
                    DocumentoID = nuevo.DocumentoID,
                    Nombre = nuevo.Nombre,
                    Url = Rutas.UrlImagenSinEnlazar(nuevo.DocumentoID)
                };
            }
            return Json(resul);
        }

        [HttpPost]
        public async Task<ActionResult> NuevoLogotipo(HttpPostedFileBase LogotipoAdjunto)
        {
            Documento nuevo = null;
            if (LogotipoAdjunto != null)
            {
                nuevo = await FileManager.NuevaImagen(LogotipoAdjunto);
            }

            object resul = null;
            if (nuevo != null)
            {
                resul = new
                {
                    DocumentoID = nuevo.DocumentoID,
                    Nombre = nuevo.Nombre,
                    Url = Rutas.UrlImagenSinEnlazar(nuevo.DocumentoID)
                };
            }
            return Json(resul);
        }

        [HttpPost]
        public async Task<ActionResult> NuevoBanner(HttpPostedFileBase BannerAdjunto)
        {
            Documento nuevo = null;
            if (BannerAdjunto != null)
            {
                nuevo = await FileManager.NuevaImagen(BannerAdjunto);
            }

            object resul = null;
            if (nuevo != null)
            {
                resul = new
                {
                    DocumentoID = nuevo.DocumentoID,
                    Nombre = nuevo.Nombre,
                    Url = Rutas.UrlImagenSinEnlazar(nuevo.DocumentoID)
                };
            }
            return Json(resul);
        }

        [OutputCache(VaryByParam = "id")]
        public async Task<ActionResult> AdjuntoSinEnlazar(long id, bool descargar = false)
        {

            IDocumentosServicio srv = Servicios.DocumentosServicio();
            Documento documento = srv.GetSingle(c => c.DocumentoID == id);

            if (documento != null)
            {
                var cd = new ContentDisposition
                {
                    FileName = documento.Nombre,
                    Inline = !descargar,
                };

                Response.Buffer = false;
                Response.Charset = "UTF-8";
                Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                Response.AddHeader(name: "Content-Length", value: documento.Tamano.ToString());
                Response.ContentType = documento.Mime;
                Response.Flush();
                await FileManager.ObtenerDocumento(documento, Response.OutputStream);
                return new EmptyResult();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [OutputCache(VaryByParam = "id")]
        public async Task<ActionResult> ImagenSinEnlazar(long id)
        {
            IDocumentosServicio srv = Servicios.DocumentosServicio();
            Documento imagen = srv.GetSingle(c => c.DocumentoID == id);

            if (imagen != null)
            {
                var cd = new ContentDisposition
                {
                    FileName = imagen.Nombre,
                    Inline = false,
                };

                Response.Buffer = false;
                Response.Charset = "UTF-8";
                Response.AppendHeader(name: "Content-Disposition", value: cd.ToString());
                Response.AddHeader(name: "Content-Length", value: imagen.Tamano.ToString());
                Response.ContentType = imagen.Mime;
                Response.Flush();
                await FileManager.ObtenerDocumento(imagen, Response.OutputStream);
                return new EmptyResult();
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}