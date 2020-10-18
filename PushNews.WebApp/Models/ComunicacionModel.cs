using System;
using System.ComponentModel.DataAnnotations;
using Txt =PushNews.WebApp.App_LocalResources;
using Entity = PushNews.Dominio.Entidades.Comunicacion;
using PushNews.WebApp.Helpers;

namespace PushNews.WebApp.Models
{
    public class ComunicacionModel
    {
        public static Func<Entity, ComunicacionModel> FromEntity =
            c => new ComunicacionModel
            {
                ComunicacionID = c.ComunicacionID,
                CategoriaID = c.CategoriaID,
                FechaPublicacion = c.FechaPublicacion,
                Titulo = c.Titulo,
                Autor = Util.AsegurarNulos(c.Autor),
                Icono = c.Categoria.Icono,
                ImagenUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlImagen(c.ComunicacionID) : null,
                MiniaturaUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlMiniatura(c.ComunicacionID) : null,
                Youtube = c.Youtube,
                Categoria = c.Categoria.Nombre,
                Destacado = c.Destacado
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

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Autor")]
        public string Autor { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Categoria")]
        public string Categoria { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Icono")]
        public string Icono { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Youtube")]
        public string Youtube { get; set; }

        public string ImagenUrl { get; set; }

        public string MiniaturaUrl { get; set; }
    }
}
