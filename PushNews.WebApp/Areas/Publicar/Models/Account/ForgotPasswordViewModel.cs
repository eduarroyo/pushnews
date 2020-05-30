using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Txt = PushNews.WebApp.App_LocalResources.Account;
using TxtVal = PushNews.WebApp.App_LocalResources.Validacion;

namespace PushNews.WebApp.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Requerido")]
        [Display(ResourceType = typeof(Txt), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(TxtVal), ErrorMessageResourceName = "Email")]
        public string Email { get; set; }
    }
}