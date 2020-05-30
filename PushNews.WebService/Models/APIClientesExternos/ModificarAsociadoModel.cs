using System;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Asociado;
using Txt = PushNews.WebService.App_LocalResources;

namespace PushNews.WebService.Models.ApiClientesExternos
{
    public class ModificarAsociadoModel: SolicitudAsociadosModel
    {
        public static readonly Func<Entity, ModificarAsociadoModel> FromEntity =
            c => new ModificarAsociadoModel()
            {
                AsociadoID = c.AsociadoID,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Apellidos = c.Apellidos,
                Direccion = c.Direccion,
                Localidad = c.Localidad,
                CodigoPostal = c.CodigoPostal,
                Telefono = c.Telefono,
                Email = c.Email,
                Provincia = c.Provincia,
                Observaciones = c.Observaciones,
                Latitud = c.Latitud,
                Longitud = c.Longitud,
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
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long AsociadoID { get; set; }

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
    }
}