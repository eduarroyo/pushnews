using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Publicar.Models.Account
{
    public class RecuperarClaveModel
    {
        [Display(ResourceType = typeof(Txt.Account), Name="Email")]
        //[EmailAddress(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName="Email")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName="Requerido")]
        public string Email { get; set; }
    }
}
