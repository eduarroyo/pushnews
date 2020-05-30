using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebService.App_LocalResources;

namespace PushNews.WebService.Models
{
    public class CambiarClaveAsociadoModel
    {
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveActual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveNueva")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveNoCoincide")]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveNuevaConfirmar")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}