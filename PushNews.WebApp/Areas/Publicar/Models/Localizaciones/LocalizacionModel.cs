using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Localizacion;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Localizaciones
{
    public class LocalizacionModel
    {
        public static Func<Entity, LocalizacionModel> FromEntity =
            c => new LocalizacionModel
            {
                LocalizacionID = c.LocalizacionID,
                Longitud = c.Longitud,
                Latitud = c.Latitud,
                Descripcion = c.Descripcion,
                Fecha = c.Fecha,
                Activo = c.Activo
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Longitud = Longitud;
            modificar.Latitud = Latitud;
            modificar.Descripcion = Descripcion.Trim();
            modificar.Fecha = Fecha;
            modificar.Activo = Activo;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long LocalizacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Localizaciones), Name = "Longitud")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double Longitud { get; set; }

        [Display(ResourceType = typeof(Txt.Localizaciones), Name = "Latitud")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double Latitud { get; set; }

        [Display(ResourceType = typeof(Txt.Localizaciones), Name = "Descripcion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(256, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Localizaciones), Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activo")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }
    }
}