using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace PushNews.WebApp.Models.Account
{
    public class CambiarClaveUsuarioModel
    {
        [HiddenInput(DisplayValue = false)]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorQue")]
        [Required(ErrorMessageResourceType=typeof(Txt.Validacion), ErrorMessageResourceName="Requerido")]
        public long UsuarioID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "Clave")]
        public string Clave { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Compare("Clave", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveNoCoincide")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveConfirmacion")]
        public string ConfirmarClave { get; set; }
    }
}
