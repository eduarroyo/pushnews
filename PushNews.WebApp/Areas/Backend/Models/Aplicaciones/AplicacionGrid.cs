using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PushNews.WebApp.Helpers;
using Entity = PushNews.Dominio.Entidades.Aplicacion;
using Txt = PushNews.WebApp.App_LocalResources;
using System.Collections.Generic;
using System.Linq;
using PushNews.WebApp.Helpers.Validation;

namespace PushNews.WebApp.Models.Aplicaciones
{
    public class AplicacionGrid
    {
        public static readonly Func<Entity, AplicacionGrid> FromEntity =
            a => new AplicacionGrid()
            {
                AplicacionID = a.AplicacionID,
                Nombre = Util.AsegurarNulos(a.Nombre),
                LogotipoID = a.LogotipoID,
                Activo = a.Activo,
                CloudKey = Util.AsegurarNulos(a.CloudKey),
                SubDominio = Util.AsegurarNulos(a.SubDominio),
                ClaveSuscripcion = a.ClaveSuscripcion,
                RequerirClaveSuscripcion = a.RequerirClaveSuscripcion,
                Usuario = Util.AsegurarNulos(a.Usuario),
                Clave = a.Clave,
                AplicacionesAmigas = a.AplicacionesAmigas.Select(aa => aa.AplicacionID),
                ApiKey = a.ApiKey,
                PermitirAccesoApiExternos = a.PermitirAccesoApiExternos,
                ApiKeyExternos = a.ApiKeyExternos,
                ITunesUrl = a.ITunesUrl,
                MicrosoftStoreUrl = a.MicrosoftStoreUrl,
                PlayStoreUrl = a.PlayStoreUrl,

                LogotipoUrl = a.Logotipo != null ? $"/Home/Logotipo/{a.AplicacionID}" : "",
                Caracteristicas = a.Caracteristicas.Select(c => c.AplicacionCaracteristicaID).ToList(),
                CaracteristicasNombres = a.Caracteristicas.Select(c => c.Nombre).ToList()
            };

        public void ActualizarEntidad(Entity entidad)
        {
            entidad.AplicacionID = AplicacionID;
            entidad.Nombre = Nombre;
            entidad.LogotipoID = LogotipoID;
            entidad.Activo = Activo;
            entidad.CloudKey = CloudKey;
            entidad.SubDominio = SubDominio;
            entidad.ClaveSuscripcion = ClaveSuscripcion;
            entidad.RequerirClaveSuscripcion = RequerirClaveSuscripcion;
            entidad.Usuario = Usuario;
            entidad.Clave = Clave;
            entidad.ApiKey = ApiKey;
            entidad.PermitirAccesoApiExternos = PermitirAccesoApiExternos;
            entidad.ApiKeyExternos = ApiKeyExternos;
            entidad.ITunesUrl = ITunesUrl;
            entidad.MicrosoftStoreUrl = MicrosoftStoreUrl;
            entidad.PlayStoreUrl = PlayStoreUrl;
            entidad.Activo = Activo;
        }

        public AplicacionGrid()
        {
            Activo = true; // Activo por defecto
        }

        [HiddenInput(DisplayValue = false)]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long AplicacionID { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(150, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Logotipo")]
        [Range(1, long.MaxValue, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "MayorIgualQue")]
        public long? LogotipoID { get; set; }
        public string LogotipoUrl { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Caracteristicas")]
        public IEnumerable<long> Caracteristicas { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Caracteristicas")]
        public IEnumerable<string> CaracteristicasNombres { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "CloudKey")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string CloudKey { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "SubDominio")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string SubDominio { get; set; }
        
        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "ClaveSuscripcion")]
        [RequiredIf("RequerirClaveSuscripcion", ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string ClaveSuscripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "RequerirClaveSuscripcion")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public bool RequerirClaveSuscripcion { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Usuario")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Usuario { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "Clave")]
        [StringLength(100, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string Clave { get; set; }

        [Display(ResourceType = typeof(Txt.Comun), Name = "Activa")]
        [HiddenInput(DisplayValue = false)]
        public bool Activo { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "AplicacionesAmigas")]
        public IEnumerable<long> AplicacionesAmigas { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "ApiKey")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string ApiKey { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "PermitirAccesoApiExternos")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        public bool PermitirAccesoApiExternos { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "ApiKeyExternos")]
        [Required(ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "Requerido")]
        [StringLength(500, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string ApiKeyExternos { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "ITunesUrl")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string ITunesUrl { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "MicrosoftStoreUrl")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string MicrosoftStoreUrl { get; set; }

        [Display(ResourceType = typeof(Txt.Aplicaciones), Name = "PlayStoreUrl")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Txt.Validacion), ErrorMessageResourceName = "CadenaLongitudMaxima")]
        public string PlayStoreUrl { get; set; }
    }
}