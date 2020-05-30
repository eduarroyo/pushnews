using System.ComponentModel.DataAnnotations;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Areas.Publicar.Models.Perfiles
{
    public class PerfilUsuarioModel
    {
        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Apellidos")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Apellidos { get; set; }
    }
}