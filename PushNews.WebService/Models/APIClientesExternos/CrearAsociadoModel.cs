using System;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Asociado;
using Txt = PushNews.WebService.App_LocalResources;

namespace PushNews.WebService.Models.ApiClientesExternos
{    
        public class CrearAsociadoModel : SolicitudAsociadosModel
        {
            public static readonly Func<Entity, CrearAsociadoModel> FromEntity =
                c => new CrearAsociadoModel()
                {
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    Apellidos = c.Apellidos,
                    Direccion = c.Direccion,
                    CodigoPostal = c.CodigoPostal,
                    Localidad = c.Localidad,
                    Provincia = c.Provincia,
                    Telefono = c.Telefono,
                    Email = c.Email,
                    Observaciones = c.Observaciones,
                    Latitud = c.Latitud,
                    Longitud = c.Longitud,
                    Clave = "foobar",
                    ConfirmarClave = "foobar"
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
            }

            [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
            [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Codigo { get; set; }

            [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
            [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Nombre { get; set; }

            [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Apellidos { get; set; }

            [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Direccion { get; set; }

            [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Localidad { get; set; }

            [StringLength(10, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string CodigoPostal { get; set; }

            [StringLength(20, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Telefono { get; set; }

            [StringLength(200, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Email { get; set; }

            [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Provincia { get; set; }

            [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
            public string Observaciones { get; set; }

            [Range(-90, 90, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
            public double? Latitud { get; set; }

            [Range(-180, 180, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
            public double? Longitud { get; set; }

            [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
            [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveLongitud")]
            public string Clave { get; set; }

            [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
            [Compare("Clave", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "ClaveNoCoincide")]
            public string ConfirmarClave { get; set; }
        }
    }