using System.Web;
using Microsoft.AspNet.Identity.Owin;
using PushNews.Dominio;
using PushNews.Negocio;
using PushNews.Negocio.Interfaces;
using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Helpers
{
    public static class Rutas
    {
        public static string Absoluta(string relativa, Aplicacion aplicacion)
        {
            IPushNewsUnitOfWork uow = HttpContext.Current.Request.GetOwinContext().Get<IPushNewsUnitOfWork>();
            var servicios = new ServiciosFactoria(uow, aplicacion);
            IParametrosServicio srv = servicios.ParametrosServicio();
            Parametro paramRaizDocumentos = srv.GetByName(nombreParametro: "RaizDocumentos");
            string rutaRaiz = paramRaizDocumentos.Valor;
            string rutaAbsoluta = System.IO.Path.Combine(rutaRaiz, relativa);
            return rutaAbsoluta;
        }

        public static string UrlAdjunto(long comunicacionID)
        {
            return $"/Home/Documento/{comunicacionID}";
        }

        public static string UrlAdjunto(Comunicacion comunicacion)
        {
            return UrlAdjunto(comunicacion.ComunicacionID);
        }

        public static string UrlAdjuntoSinEnlazar(long documentoID)
        {
            return $"Backend/Adjuntos/AdjuntoSinEnlazar/{documentoID}";
        }

        public static string UrlImagen(long comunicacionID)
        {
            return $"/Home/Imagen/{comunicacionID}";
        }

        public static string UrlImagen(Comunicacion comunicacion)
        {
            return UrlImagen(comunicacion.ComunicacionID);
        }

        public static string UrlMiniatura(Comunicacion comunicacion)
        {
            return UrlMiniatura(comunicacion.ComunicacionID);
        }

        public static string UrlMiniatura(long comunicacionID)
        {
            return $"/Home/Miniatura/{comunicacionID}";
        }

        public static string UrlImagenSinEnlazar(long documentoID)
        {
            return $"Backend/Adjuntos/ImagenSinEnlazar/{documentoID}";
        }

        public static string UrlLogotipo(long aplicacionID)
        {
            return $"Home/Logotipo/{aplicacionID}";
        }

        public static string UrlLogotipoEmpresa(Empresa empresa)
        {
            return UrlLogotipoEmpresa(empresa.EmpresaID);
        }
        public static string UrlLogotipoEmpresa(long empresaId)
        {
            return $"Home/LogotipoEmpresa/{empresaId}";
        }

        public static string UrlBannerEmpresa(Empresa empresa)
        {
            return UrlBannerEmpresa(empresa.EmpresaID);
        }
        public static string UrlBannerEmpresa(long empresaId)
        {
            return $"/Home/BannerEmpresa/{empresaId}";
        }

        public static string UrlLogotipo(Aplicacion aplicacion)
        {
            return UrlLogotipo(aplicacion.AplicacionID);
        }

        public static string UrlComunicacion(Comunicacion comunicacion)
        {
            return $"#/{comunicacion.ComunicacionID}";
        }

        public static string FacebookShare(Comunicacion comunicacion)
        {
            return string.Format(
                "http://www.facebook.com/sharer/sharer.php?u={0}&title={1}",
                RutaAbsolutaComunicacion(comunicacion), comunicacion.Titulo);
        }

        public static string TwitterShare(Comunicacion comunicacion)
        {
            return string.Format(
                "http://twitter.com/intent/tweet?status={0}", RutaAbsolutaComunicacion(comunicacion));
        }

        public static string RutaAbsolutaComunicacion(Comunicacion comunicacion)
        {
            return $"http://{comunicacion.Categoria.Aplicacion.SubDominio}.pushnews.com/{comunicacion.ComunicacionID}";
        }
    }
}