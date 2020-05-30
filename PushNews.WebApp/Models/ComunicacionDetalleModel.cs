using System;
using System.ComponentModel.DataAnnotations;
using Txt =PushNews.WebApp.App_LocalResources;
using Entity = PushNews.Dominio.Entidades.Comunicacion;
using PushNews.WebApp.Helpers;

namespace PushNews.WebApp.Models
{
    public class ComunicacionDetalleModel
    {
        public static Func<Entity, ComunicacionDetalleModel> FromEntity =
            c => new ComunicacionDetalleModel
            {
                ComunicacionID = c.ComunicacionID,
                CategoriaID = c.CategoriaID,
                FechaPublicacion = c.FechaPublicacion,
                Titulo = c.Titulo,
                Descripcion = c.Descripcion,
                Autor = Util.AsegurarNulos(c.Autor),
                ImagenTitulo = Util.AsegurarNulos(c.ImagenTitulo),
                ImagenDocumentoID = c.ImagenDocumentoID,
                ImagenUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlImagen(c.ComunicacionID) : "",
                MiniaturaUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlMiniatura(c.ComunicacionID) : "",
                ImagenNombre = c.Imagen?.Nombre,
                AdjuntoTitulo = Util.AsegurarNulos(c.AdjuntoTitulo),
                AdjuntoDocumentoID = c.AdjuntoDocumentoID,
                AdjuntoUrl = c.AdjuntoDocumentoID.HasValue ? Helpers.Rutas.UrlAdjunto(c.ComunicacionID) : "",
                AdjuntoNombre = c.Adjunto?.Nombre,
                EnlaceTitulo = Util.AsegurarNulos(c.EnlaceTitulo),
                Enlace = c.Enlace,
                YoutubeTitulo = c.YoutubeTitulo,
                Youtube = c.Youtube,
                GeoPosicionTitulo = c.GeoPosicionTitulo,
                GeoPosicionLatitud = c.GeoPosicionLatitud,
                GeoPosicionLongitud = c.GeoPosicionLongitud,
                GeoPosicionDireccion = c.GeoPosicionDireccion,
                GeoPosicionLocalidad = c.GeoPosicionLocalidad,
                GeoPosicionProvincia = c.GeoPosicionProvincia,
                GeoPosicionPais = c.GeoPosicionPais,
                Destacado = c.Destacado,

                FacebookShare = Helpers.Rutas.FacebookShare(c),
                TwitterShare = Helpers.Rutas.TwitterShare(c),
                Url = Helpers.Rutas.RutaAbsolutaComunicacion(c),
                Categoria = c.Categoria.Nombre,
                Privada = c.Categoria.Privada

            };

        public long ComunicacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Categoria")]
        public long CategoriaID { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Destacado")]
        public bool Destacado { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "FechaPublicacion")]
        public DateTime FechaPublicacion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Titulo")]
        public string Titulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Autor")]
        public string Autor { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "ImagenTitulo")]
        public string ImagenTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Imagen")]
        public long? ImagenDocumentoID { get; set; }

        public string ImagenUrl { get; set; }
        public string MiniaturaUrl { get; set; }
        public string ImagenNombre { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "AdjuntoTitulo")]
        public string AdjuntoTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Adjunto")]
        public long? AdjuntoDocumentoID { get; set; }

        public string AdjuntoUrl { get; set; }
        public string AdjuntoNombre { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "EnlaceTitulo")]
        public string EnlaceTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Enlace")]
        public string Enlace { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "YoutubeTitulo")]
        public string YoutubeTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Youtube")]
        public string Youtube { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionTitulo")]
        public string GeoPosicionTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionLatitud")]
        public float? GeoPosicionLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionLongitud")]
        public float? GeoPosicionLongitud { get; set; }

        public string GeoPosicionDireccion { get; set; }

        public string GeoPosicionLocalidad { get; set; }

        public string GeoPosicionProvincia { get; set; }

        public string GeoPosicionPais { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Categoria")]
        public string Categoria { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Icono")]
        public string Icono { get; set; }

        public string FacebookShare { get; set; }
        public string TwitterShare { get; set; }
        public string Url { get; set; }
        public bool Privada { get; set; }
    }
}
