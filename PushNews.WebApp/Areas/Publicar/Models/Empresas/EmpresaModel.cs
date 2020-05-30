using PushNews.WebApp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using Entity = PushNews.Dominio.Entidades.Empresa;
using Txt = PushNews.WebApp.App_LocalResources;

namespace PushNews.WebApp.Models.Empresas
{
    public class EmpresaModel
    {
        public static Func<Entity, EmpresaModel> FromEntity =
            e => new EmpresaModel
            {
                EmpresaID = e.EmpresaID,
                Nombre = e.Nombre,
                Direccion = e.Direccion,
                Localidad = e.Localidad,
                CodigoPostal = e.CodigoPostal,
                Provincia = e.Provincia,
                Latitud = e.Latitud,
                Longitud = e.Longitud,
                Telefono = e.Telefono,
                Email = e.Email,
                Web = e.Web,
                Facebook = e.Facebook,
                Twitter = e.Twitter,
                LogotipoDocumentoID = e.LogotipoDocumentoID,
                BannerDocumentoID = e.BannerDocumentoID,
                Descripcion = e.Descripcion,
                Tags = e.Tags,
                Activo = e.Activo,
                LogotipoUrl = Helpers.Rutas.UrlLogotipoEmpresa(e.EmpresaID),
                BannerUrl = Helpers.Rutas.UrlBannerEmpresa(e.EmpresaID)
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Nombre = Nombre.Trim();
            modificar.Direccion = Util.AsegurarNulos(Direccion);
            modificar.Localidad = Util.AsegurarNulos(Localidad);
            modificar.CodigoPostal = Util.AsegurarNulos(CodigoPostal);
            modificar.Provincia = Util.AsegurarNulos(Provincia);
            modificar.Latitud = Latitud;
            modificar.Longitud = Longitud;
            modificar.Telefono = Util.AsegurarNulos(Telefono);
            modificar.Email = Util.AsegurarNulos(Email);
            modificar.Web = Util.AsegurarNulos(Web);
            modificar.Facebook = Util.AsegurarNulos(Facebook);
            modificar.Twitter = Util.AsegurarNulos(Twitter);
            modificar.LogotipoDocumentoID = LogotipoDocumentoID;
            modificar.BannerDocumentoID = BannerDocumentoID;
            modificar.Descripcion = Util.AsegurarNulos(Descripcion);
            modificar.Tags = Util.AsegurarNulos(Tags);
            modificar.Activo = Activo;
        }

        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long EmpresaID { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Direccion")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Direccion { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Localidad")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "CodigoPostal")]
        [StringLength(10, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Provincia")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Latitud")]
        [Range(-90D, 90D, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? Latitud { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Longitud")]
        [Range(-180D, 180D, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Intervalo")]
        public double? Longitud { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Telefono")]
        [StringLength(20, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Telefono { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Email")]
        [StringLength(250, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Web")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Web { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Facebook")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Facebook { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Twitter")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Twitter { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Logotipo")]
        public long? LogotipoDocumentoID { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Banner")]
        public long? BannerDocumentoID { get; set; }

        [Display(ResourceType = typeof(Txt.Empresas), Name = "Descripcion")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Descripcion { get; set; }

        public string LogotipoUrl { get; set; }

        public string BannerUrl { get; set; }

[Display(ResourceType = typeof(Txt.Empresas), Name = "Tags")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Tags { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name="Activa")]
        public bool Activo { get; set; }
    }
}