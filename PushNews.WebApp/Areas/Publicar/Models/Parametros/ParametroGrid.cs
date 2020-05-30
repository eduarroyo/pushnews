using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Parametro;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Parametros
{
    public class ParametroGrid
    {
        public static Func<Entity, ParametroGrid> FromEntity =
            r => new ParametroGrid()
            {
                ParametroID = r.ParametroID,
                Nombre = Util.AsegurarNulos(r.Nombre),
                Valor = Util.AsegurarNulos(r.Valor),
                Descripcion = Util.AsegurarNulos(r.Descripcion),
                AplicacionID = r.AplicacionID ?? 0,

                Aplicacion = r.Aplicacion?.Nombre
            };

        public void ActualizarEntidad(Entity nuevo)
        {
            nuevo.Nombre = Nombre;
            nuevo.Valor = Valor ?? "";
            nuevo.Descripcion = Descripcion ?? "";
            nuevo.AplicacionID = AplicacionID <= 0 ? (long?) null : AplicacionID;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long ParametroID { get; set; }


        [Display(ResourceType = typeof(Txt.Parametros), Name = "Aplicacion")]
        [Range(0, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Seleccion")]
        public long AplicacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Parametros), Name = "Aplicacion")]
        public string Aplicacion { get; set; }

        [Display(ResourceType = typeof(Txt.Parametros), Name = "Parametro")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Parametros), Name = "Valor")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Valor { get; set; }

        [Display(ResourceType = typeof(Txt.Parametros), Name = "Descripcion")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Descripcion { get; set; }
    }
}