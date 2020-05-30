using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Gps;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Gpss
{
    public class GpsModel
    {
        public static Func<Entity, double, GpsModel> FromEntity =
            (c, bateriaMinimo) => new GpsModel
            {
                GpsID = c.GpsID,
                GpsApiID = c.GpsApiID,
                Api = c.Api,
                Activo = c.Activo,
                Matricula = c.Matricula,
                Estado = c.Estado,
                Bateria = c.Bateria,
                Sensores = c.Sensores,
                UltimaLecturaFecha = c.UltimaLecturaFecha,
                UltimaPosicionFecha = c.UltimaPosicionFecha,
                UltimaPosicionDireccion = c.UltimaPosicionDireccion,
                UltimaPosicionLatitud = c.UltimaPosicionLatitud,
                UltimaPosicionLongitud = c.UltimaPosicionLongitud
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Activo = Activo;
            modificar.GpsApiID = GpsApiID;
            modificar.Api = Api;
            modificar.Matricula = Util.AsegurarNulos(Matricula);
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long GpsID { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "GpsApiID")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        //[Range(1, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long GpsApiID { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "Api")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Api { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "Matricula")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "Estado")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "Bateria")]
        public double? Bateria { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "Sensores")]
        public string Sensores { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "UltimaLecturaFecha")]
        public DateTime? UltimaLecturaFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "UltimaPosicionFecha")]
        public DateTime? UltimaPosicionFecha { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "UltimaPosicionDireccion")]
        public string UltimaPosicionDireccion { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "UltimaPosicionLatitud")]
        public double? UltimaPosicionLatitud { get; set; }

        [Display(ResourceType = typeof(Txt.Gpss), Name = "UltimaPosicionLongitud")]
        public double? UltimaPosicionLongitud { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activo")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }
    }
}