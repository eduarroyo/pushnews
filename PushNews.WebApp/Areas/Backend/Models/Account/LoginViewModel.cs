using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources.Account;
using TxtVal = PushNews.WebApp.App_LocalResources.Validacion;

namespace PushNews.WebApp.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType=typeof(TxtVal), ErrorMessageResourceName="Requerido")]
        [Display(ResourceType = typeof(Txt), Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Requerido")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt), Name = "Clave")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Txt), Name = "Recordar")]
        public bool RememberMe { get; set; }
    }
}