using PushNews.Dominio.Enums;
using System;
using Entity = PushNews.Dominio.Entidades.Comunicacion;

namespace PushNews.WebService.Models
{
    public class ComunicacionDetalleModel
    {
        public static Func<Entity, ComunicacionDetalleModel> FromEntity =
            c => new ComunicacionDetalleModel
            {
                ComunicacionID = c.ComunicacionID,
                CategoriaID = c.CategoriaID,
                Destacado = c.Destacado,
                FechaPublicacion = c.FechaPublicacion,
                Titulo = c.Titulo,
                Descripcion = c.Descripcion,
                Autor = c.Autor,
                ImagenTitulo = c.ImagenTitulo,
                ImagenDocumentoID = c.ImagenDocumentoID,
                ImagenUrl = c.ImagenDocumentoID.HasValue ? $"Home/Imagen/{c.ComunicacionID}" : "",
                MiniaturaUrl = c.ImagenDocumentoID.HasValue ? $"Home/Miniatura/{c.ComunicacionID}" : "",
                AdjuntoTitulo = c.AdjuntoTitulo,
                AdjuntoDocumentoID = c.AdjuntoDocumentoID,
                AdjuntoUrl = c.AdjuntoDocumentoID.HasValue ? $"Home/Documento/{c.ComunicacionID}" : "",
                EnlaceTitulo = c.EnlaceTitulo,
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
                Activo = c.Activo,
                TimeStamp = c.TimeStamp,
                Categoria = c.Categoria.Nombre,
                Orden = c.Categoria.Orden,
                Icono = c.Categoria.Icono
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.CategoriaID = CategoriaID;
            modificar.Destacado = Destacado;
            modificar.FechaPublicacion = FechaPublicacion;
            modificar.Titulo = Titulo;
            modificar.Descripcion = Descripcion;
            modificar.Autor = Autor;
            modificar.ImagenTitulo = ImagenTitulo;
            modificar.ImagenDocumentoID = ImagenDocumentoID;
            modificar.AdjuntoTitulo = AdjuntoTitulo;
            modificar.AdjuntoDocumentoID = AdjuntoDocumentoID;
            modificar.EnlaceTitulo = EnlaceTitulo;
            modificar.Enlace = Enlace;
            modificar.YoutubeTitulo = YoutubeTitulo;
            modificar.Youtube = Youtube;
            modificar.GeoPosicionTitulo = GeoPosicionTitulo;
            modificar.GeoPosicionLatitud = GeoPosicionLatitud;
            modificar.GeoPosicionLongitud = GeoPosicionLongitud;
            modificar.Activo = Activo;
        }

        public int Orden { get; set; }

        public long ComunicacionID { get; set; }

        public long CategoriaID { get; set; }

        public bool Destacado { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Autor { get; set; }

        public string ImagenTitulo { get; set; }

        public long? ImagenDocumentoID { get; set; }
        public string ImagenUrl { get; set; }
        public string MiniaturaUrl { get; set; }

        public string AdjuntoTitulo { get; set; }

        public long? AdjuntoDocumentoID { get; set; }
        public string AdjuntoUrl { get; set; }

        public string EnlaceTitulo { get; set; }

        public string Enlace { get; set; }

        public string YoutubeTitulo { get; set; }

        public string Youtube { get; set; }

        public string GeoPosicionTitulo { get; set; }

        public float? GeoPosicionLatitud { get; set; }

        public float? GeoPosicionLongitud { get; set; }
        public string GeoPosicionDireccion { get; set; }
        public string GeoPosicionLocalidad { get; set; }
        public string GeoPosicionProvincia { get; set; }
        public string GeoPosicionPais { get; set; }

        public string Categoria { get; set; }

        public string Icono { get; set; }

        public bool Activo { get; set; }

        public long TimeStamp { get; set; }
    }
}