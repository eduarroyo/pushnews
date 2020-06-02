using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Hermandad;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Hermandades
{
    public class HermandadModel
    {
        public static Func<Entity, HermandadModel> FromEntity =
            c => new HermandadModel
            {
                HermandadID = c.HermandadID,
                Nombre = c.Nombre,
                LogotipoDocumentoID = c.LogotipoDocumentoID,
                LogotipoUrl = Helpers.Rutas.UrlLogotipoHermandad(c.HermandadID),
                MiniaturaUrl = Helpers.Rutas.UrlMiniaturaHermandad(c.HermandadID),
                IglesiaNombre = c.IglesiaNombre,
                IglesiaDireccion = c.IglesiaDireccion,
                IglesiaLatitud = c.IglesiaLatitud,
                IglesiaLongitud = c.IglesiaLongitud,
                Activo = c.Activo,
                Tags = c.Tags

            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Nombre = Util.AsegurarNulos(Nombre);
            modificar.LogotipoDocumentoID =  LogotipoDocumentoID;
            modificar.IglesiaNombre = Util.AsegurarNulos(IglesiaNombre);
            modificar.IglesiaDireccion = Util.AsegurarNulos(IglesiaDireccion);
            modificar.IglesiaLatitud = IglesiaLatitud;
            modificar.IglesiaLongitud = IglesiaLongitud;
            modificar.Activo = Activo;
            modificar.Tags = Util.AsegurarNulos(Tags);
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long HermandadID { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "IglesiaNombre")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string IglesiaNombre { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "IglesiaDireccion")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string IglesiaDireccion { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "IglesiaLatitud")]
        [Range(-90D, 90D, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? IglesiaLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "IglesiaLongitud")]
        [Range(-180D, 180D, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? IglesiaLongitud { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activo")]
        public bool Activo { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "Logotipo")]
        public long? LogotipoDocumentoID { get; set; }

        public string LogotipoUrl { get; set; }

        public string MiniaturaUrl { get; set; }

        [Display(ResourceType = typeof(Txt.Hermandades), Name = "Tags")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Tags { get; set; }
    }
}