using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Usuario;
using Txt = PushNews.WebApp.App_LocalResources;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;
using System.Collections.Generic;
using PushNews.WebApp.Helpers;

namespace PushNews.WebApp.Models.Usuarios
{
    public class UsuarioGrid
    {
        public static readonly Func<Entity, UsuarioGrid> FromEntity =
            u => new UsuarioGrid()
            {
                UsuarioID = u.UsuarioID,
                Nombre = u.Nombre,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Activo = u.Activo,
                Clave = "foobar",
                ConfirmarClave = "foobar",
                ApellidosNombre = u.ApellidosNombre,

                // Propiedades de entidades relacionadas.
                PerfilNombre = u.Rol.Nombre,
                PerfilID = u.Rol.RolID,
                AplicacionesNombres = string.Join(separator: ", ", values: u.Aplicaciones.Select(cl => cl.Nombre)),
                Aplicaciones = u.Aplicaciones.Select(p => p.AplicacionID).ToList(),
            };

        public void ActualizarEntidad(Entity entidad)
        {
            entidad.Email = Email;
            entidad.Nombre = Nombre;
            entidad.Apellidos = Util.AsegurarNulos(Apellidos);
            entidad.Activo = Activo;

            // La clave no se actualiza aquí porque se cambia mediante una acción específica.
        }

        public UsuarioGrid()
        {
            Activo = true; // Activo por defecto
        }

        [HiddenInput(DisplayValue = false)]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long UsuarioID { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(250, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Apellidos")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Apellidos { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Nombre")]
        public string ApellidosNombre { get; private set; }
        
        [Display(ResourceType = typeof(Txt.Comun), Name = "Activo")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "Clave")]
        public string Clave { get; set; }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Compare("Clave", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveNoCoincide")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Txt.Account), Name = "ClaveConfirmacion")]
        public string ConfirmarClave { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Perfil")]
        public string PerfilNombre { get; private set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Perfil")]
        public long PerfilID { get; set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Aplicaciones")]
        public string AplicacionesNombres { get; private set; }

        [Display(ResourceType = typeof(Txt.Usuarios), Name = "Aplicaciones")]
        public IEnumerable<long> Aplicaciones { get; set; }
    }
}