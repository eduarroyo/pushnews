using System.ComponentModel.DataAnnotations;
using TxtVal = PushNews.WebApp.App_LocalResources.Validacion;
using Txt = PushNews.WebApp.App_LocalResources.Account;

namespace PushNews.WebApp.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceType=typeof(TxtVal), ErrorMessageResourceName="Requerido")]
        [Display(ResourceType = typeof(Txt), Name = "Usuario")]
        public string UserName { get; set; }
    }
}