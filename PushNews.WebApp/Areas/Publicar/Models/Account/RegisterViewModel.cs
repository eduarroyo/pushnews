using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources.Account;
using TxtVal = PushNews.WebApp.App_LocalResources.Validacion;

namespace PushNews.WebApp.Models.Account
{
    public class RegisterViewModel
    {
        [Display(ResourceType = typeof(Txt), Name = "Usuario")]
        [Required(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Requerido")]
        [StringLength(50, ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Usuario { get; set; }

        [Display(ResourceType = typeof(Txt), Name = "Email")]
        [Required(ErrorMessageResourceType=typeof(TxtVal), ErrorMessageResourceName="Requerido")]
        //[EmailAddress(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(TxtVal),
            ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt), Name = "Clave")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt), Name = "ClaveConfirmacion")]
        [Compare("Password", ErrorMessageResourceType = typeof(TxtVal),
            ErrorMessageResourceName = "ClaveNoCoincide")]
        public string ConfirmPassword { get; set; }
    }
}