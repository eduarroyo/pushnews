using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entity = PushNews.Dominio.Entidades.Asociado;
using Txt = PushNews.WebApp.App_LocalResources;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;


namespace PushNews.WebApp.Models.Asociados
{
    public class AsociadoModel
    {
        public static readonly Func<Entity, AsociadoModel> FromEntity =
            c => new AsociadoModel()
            {
                AsociadoID = c.AsociadoID,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Apellidos = c.Apellidos,
                Direccion = c.Direccion,
                CodigoPostal = c.CodigoPostal,
                Localidad = c.Localidad,
                Provincia = c.Provincia,
                Telefono = c.Telefono,
                Email = c.Email,
                Activo = c.Activo,
                Clave = "foobar",
                ConfirmarClave = "foobar",

                ApellidosNombre = string.IsNullOrWhiteSpace(c.Apellidos) ? c.Nombre : $"{c.Apellidos}, {c.Nombre}",
                NombreApellidos = string.IsNullOrWhiteSpace(c.Apellidos) ? c.Nombre : $"{c.Nombre} {c.Apellidos}",
                Aplicacion = c.Aplicacion.Nombre,
            };

        public void ActualizarEntidad(Entity entidad)
        {
            entidad.Codigo = Codigo;
            entidad.Nombre = Nombre;
            entidad.Apellidos = Apellidos;
            entidad.Direccion = Direccion;
            entidad.Localidad = Localidad;
            entidad.CodigoPostal = CodigoPostal;
            entidad.Telefono = Telefono;
            entidad.Email = Email;
            entidad.Provincia = Provincia;
            entidad.Observaciones = Observaciones;
            entidad.Latitud = Latitud;
            entidad.Longitud = Longitud;
            entidad.Activo = Activo;
        }

        [HiddenInput(DisplayValue = false)]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long AsociadoID { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Codigo")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Codigo { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Apellidos")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Apellidos { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Nombre")]
        public string ApellidosNombre { get; private set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Nombre")]
        public string NombreApellidos { get; private set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Direccion")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Direccion { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Localidad")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "CodigoPostal")]
        [StringLength(10, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Telefono")]
        [StringLength(20, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Telefono { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Email")]
        [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Provincia")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Observaciones")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Observaciones { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Latitud")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? Latitud { get; set; }

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Longitud")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? Longitud { get; set; }

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

        [Display(ResourceType = typeof(Txt.Asociados), Name = "Aplicacion")]
        public string Aplicacion { get; set; }
    }
}