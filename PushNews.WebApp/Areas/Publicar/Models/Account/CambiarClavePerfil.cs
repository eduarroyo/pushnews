using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Txt = PushNews.WebApp.App_LocalResources;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace PushNews.WebApp.Models.Account
{
    public class CambiarClaveModel
    {
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveActual")]
        public string ClaveActual { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveNueva")]
        public string ClaveNueva { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Compare("ClaveNueva", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveNoCoincide")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveConfirmacion")]
        public string ConfirmarClave { get; set; }
    }
}
