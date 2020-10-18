using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Categoria;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models
{
    public class CategoriaModel
    {
        public static Func<Entity, CategoriaModel> FromEntity =
            r => new CategoriaModel()
            {
                CategoriaID = r.CategoriaID,
                Nombre = Util.AsegurarNulos(r.Nombre),
                Icono = Util.AsegurarNulos(r.Icono),
                Orden = r.Orden,
                Activo = r.Activo,
                
            };

        public void ActualizarEntidad(Entity editar)
        {
            editar.Nombre = Nombre;
            editar.Icono = Icono;
            editar.Activo = Activo;
            editar.Orden = Orden;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long CategoriaID { get; set; }

        [Display(ResourceType = typeof(Txt.Categorias), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Categorias), Name = "Icono")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Icono { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activa")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }

        [Display(ResourceType = typeof(Txt.Categorias), Name = "Orden")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(1, int.MaxValue,ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        [UIHint("Integer")]
        public int Orden { get; set; }
    }
}