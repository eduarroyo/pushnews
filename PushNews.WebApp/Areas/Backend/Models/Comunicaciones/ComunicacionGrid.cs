using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Comunicacion;
using Txt = PushNews.WebApp.App_LocalResources;
using PushNews.WebApp.Helpers;
using PushNews.Dominio.Enums;
using PushNews.WebApp.Helpers.Validation;
using System.Collections.Generic;

namespace PushNews.WebApp.Models.Comunicaciones
{
    public class ComunicacionGrid
    {
        public static Func<Entity, IEnumerable<long>, double, ComunicacionGrid> FromEntity =
            (c, categoriasPermitidas, horasEnvio) =>
            {
                DateTime haceUnMes = DateTime.Now.AddMonths(-1);
                return new ComunicacionGrid
                {
                    ComunicacionID = c.ComunicacionID,
                    CategoriaID = c.CategoriaID,
                    FechaCreacion = c.FechaCreacion,
                    FechaPublicacion = c.FechaPublicacion,
                    Titulo = c.Titulo,
                    Descripcion = c.Descripcion,
                    Autor = c.Autor,
                    ImagenTitulo = c.ImagenTitulo,
                    ImagenDocumentoID = c.ImagenDocumentoID,
                    ImagenUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlImagen(c.ComunicacionID) : "",
                    MiniaturaUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlMiniatura(c.ComunicacionID) : "",
                    ImagenNombre = c.Imagen?.Nombre,
                    AdjuntoTitulo = c.AdjuntoTitulo,
                    AdjuntoDocumentoID = c.AdjuntoDocumentoID,
                    AdjuntoUrl = c.AdjuntoDocumentoID.HasValue ? Helpers.Rutas.UrlAdjunto(c.ComunicacionID) : "",
                    AdjuntoNombre = c.Adjunto?.Nombre,
                    EnlaceTitulo = c.EnlaceTitulo,
                    Enlace = Util.AsegurarNulos(c.Enlace),
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
                    EstadoPublicacion = c.EstadoPublicacion(horasEnvio),
                    Destacado = c.Destacado,
                    Recordatorio = c.RecordatorioFecha.HasValue,
                    RecordatorioFecha = c.RecordatorioFecha,
                    RecordatorioTitulo = c.RecordatorioTitulo,

                    EdicionPermitida = categoriasPermitidas == null
                                        || categoriasPermitidas.Any(catID => catID == c.CategoriaID),

                    Categoria = c.Categoria.Nombre,
                    Icono = c.Categoria.Icono,

                    Visualizaciones = c.Accesos.Count(),
                    VisualizacionesUltimoMes = c.Accesos.Where(a => a.Fecha >= haceUnMes).Count(),
                    Terminales = c.Accesos.Select(acc => acc.Terminal).Distinct().Count()
                };
            };

        public static Func<Entity, IEnumerable<long>, double, ComunicacionGrid> FromEntityParaAdmin =
            (c, categoriasPermitidas, horasEnvio) =>
            {
                DateTime haceUnMes = DateTime.Now.AddMonths(-1);
                return new ComunicacionGrid
                {
                    ComunicacionID = c.ComunicacionID,
                    CategoriaID = c.CategoriaID,
                    FechaCreacion = c.FechaCreacion,
                    FechaPublicacion = c.FechaPublicacion,
                    Titulo = c.Titulo,
                    Descripcion = c.Descripcion,
                    Autor = c.Autor,
                    ImagenTitulo = c.ImagenTitulo,
                    ImagenDocumentoID = c.ImagenDocumentoID,
                    ImagenUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlImagen(c.ComunicacionID) : "",
                    MiniaturaUrl = c.ImagenDocumentoID.HasValue ? Helpers.Rutas.UrlMiniatura(c.ComunicacionID) : "",
                    ImagenNombre = c.Imagen?.Nombre,
                    AdjuntoTitulo = c.AdjuntoTitulo,
                    AdjuntoDocumentoID = c.AdjuntoDocumentoID,
                    AdjuntoUrl = c.AdjuntoDocumentoID.HasValue ? Helpers.Rutas.UrlAdjunto(c.ComunicacionID) : "",
                    AdjuntoNombre = c.Adjunto?.Nombre,
                    EnlaceTitulo = c.EnlaceTitulo,
                    Enlace = Util.AsegurarNulos(c.Enlace),
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
                    PushFecha = c.PushFecha,
                    EstadoPublicacion = c.EstadoPublicacion(horasEnvio),
                    Destacado = c.Destacado,
                    Recordatorio = c.RecordatorioFecha.HasValue,
                    RecordatorioFecha = c.RecordatorioFecha,
                    RecordatorioTitulo = c.RecordatorioTitulo,

                    EdicionPermitida = categoriasPermitidas == null
                                        || categoriasPermitidas.Any(catID => catID == c.CategoriaID),

                    Categoria = c.Categoria.Nombre,
                    Icono = c.Categoria.Icono,
                    Visualizaciones = c.Accesos.Count(),
                    VisualizacionesUltimoMes = c.Accesos.Where(a => a.Fecha >= haceUnMes).Count(),
                    Terminales = c.Accesos.Select(acc => acc.Terminal).Distinct().Count()
                };
            };

        public void ActualizarEntidad(Entity modificar, bool mantenerUbicacion)
        {
            modificar.TimeStamp = PushNews.Negocio.Util.UnixTimeStamp(DateTime.Now);
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
            if (Recordatorio)
            {
                modificar.RecordatorioFecha = RecordatorioFecha;
                modificar.RecordatorioTitulo = RecordatorioTitulo;
            }
            if (mantenerUbicacion)
            {
                modificar.GeoPosicionTitulo = GeoPosicionTitulo;
                modificar.GeoPosicionLatitud = GeoPosicionLatitud;
                modificar.GeoPosicionLongitud = GeoPosicionLongitud;
                modificar.GeoPosicionDireccion = GeoPosicionDireccion;
                modificar.GeoPosicionLocalidad = GeoPosicionLocalidad;
                modificar.GeoPosicionProvincia = GeoPosicionProvincia;
                modificar.GeoPosicionPais = GeoPosicionPais;
            }
            else
            {
                modificar.GeoPosicionTitulo = null;
                modificar.GeoPosicionLatitud = null;
                modificar.GeoPosicionLongitud = null;
                modificar.GeoPosicionDireccion = null;
                modificar.GeoPosicionLocalidad = null;
                modificar.GeoPosicionProvincia = null;
                modificar.GeoPosicionPais = null;
            }
            modificar.Activo = Activo;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long ComunicacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Categoria")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(1, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Seleccion")]
        public long CategoriaID { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Destacado")]
        public bool Destacado { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "EstadoPublicacion")]
        public EstadosPublicacion EstadoPublicacion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "EstadoPublicacion")]
        public string EstadoPublicacionTxt { get { return Textos.EstadoComunicacion(this.EstadoPublicacion); } }

        public string EstadoPublicacionTitle { get { return Textos.EstadoComunicacionTitle(this.EstadoPublicacion); } }

        public string EstadoIcono { get { return Textos.EstadoComunicacionIcono(this.EstadoPublicacion); } }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "FechaPublicacion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public DateTime FechaPublicacion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Titulo")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Titulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Descripcion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Autor")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Autor { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name ="Recordatorio")]
        public bool Recordatorio { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "RecordatorioFecha")]
        [RequiredIf("Recordatorio", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public DateTime? RecordatorioFecha { get; set; }
        
        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "RecordatorioTitulo")]
        [RequiredIf("Recordatorio", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string RecordatorioTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "ImagenTitulo")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string ImagenTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name="Imagen")]
        [Range(1, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long? ImagenDocumentoID { get; set; }

        public string ImagenUrl{ get; set; }
        public string MiniaturaUrl { get; set; }
        public string ImagenNombre { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "AdjuntoTitulo")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string AdjuntoTitulo { get; set; }


        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Adjunto")]
        [Range(1, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long? AdjuntoDocumentoID { get; set; }

        public string AdjuntoUrl { get; set; }
        public string AdjuntoNombre { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "EnlaceTitulo")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string EnlaceTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "Enlace")]
        //[Url(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Url", ErrorMessage = null)]
        //[DataType(DataType.Url)]
        public string Enlace { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "YoutubeTitulo")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string YoutubeTitulo { get; set; }

        public string Youtube { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionTitulo")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string GeoPosicionTitulo { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionLatitud")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public float? GeoPosicionLatitud { get; set; }


        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "GeoPosicionLongitud")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public float? GeoPosicionLongitud { get; set; }

        [Display(ResourceType = typeof(Txt.Comunicaciones), Name = "PushFecha")]
        public DateTime? PushFecha { get; set; }
        
        public string GeoPosicionDireccion { get; set; }

        public string GeoPosicionLocalidad { get; set; }

        public string GeoPosicionProvincia { get; set; }

        public string GeoPosicionPais { get; set; }

        public string Categoria { get; set; }
        public string Icono { get; set; }
        
        public bool Activo { get; set; }

        public int Terminales { get; set; }
        public int VisualizacionesUltimoMes { get; set; }
        public int Visualizaciones { get; set; }

        public bool EdicionPermitida { get; set; }
        public bool Privada { get; set; }
    }
}