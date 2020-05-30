using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources.Account;
using TxtVal = PushNews.WebApp.App_LocalResources.Validacion;

namespace PushNews.WebApp.Models.Account
{
    public class ManageUserViewModel
    {
        [Required()]
        [Range(1, long.MaxValue)]
        public long UsuarioID { get; set; }

        [Required(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(TxtVal),
            ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt), Name = "ClaveNueva")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt), Name = "ClaveConfirmacion")]
        [Compare("Clave", ErrorMessageResourceType = typeof(TxtVal),
            ErrorMessageResourceName = "ClaveNoCoincide")]
        public string ConfirmPassword { get; set; }
    }
}