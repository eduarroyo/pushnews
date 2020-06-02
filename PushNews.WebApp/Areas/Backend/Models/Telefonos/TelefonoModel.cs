using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Telefono;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Telefonos
{
    public class TelefonoModel
    {
        public static Func<Entity, TelefonoModel> FromEntity =
            c => new TelefonoModel
            {
                TelefonoID = c.TelefonoID,
                Numero = c.Numero,
                Descripcion = c.Descripcion,
                Fecha = c.Fecha,
                Activo = c.Activo
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Numero = Numero.Trim();
            modificar.Descripcion = Descripcion.Trim();
            modificar.Fecha = Fecha;
            modificar.Activo = Activo;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long TelefonoID { get; set; }

        [Display(ResourceType = typeof(Txt.Telefonos), Name = "Numero")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Numero { get; set; }

        [Display(ResourceType = typeof(Txt.Telefonos), Name = "Descripcion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(256, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Descripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activo")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }

        [Display(ResourceType = typeof(Txt.Telefonos), Name = "Fecha")]
        public DateTime Fecha { get; set; }
    }
}