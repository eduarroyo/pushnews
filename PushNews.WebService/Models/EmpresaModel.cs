using System;
using Entity = PushNews.Dominio.Entidades.Empresa;

namespace PushNews.WebService.Models
{
    public class EmpresaModel
    {
        public static Func<Entity, EmpresaModel> FromEntity =
            c => new EmpresaModel
            {
                EmpresaID = c.EmpresaID,
                Nombre = c.Nombre,
                Direccion = c.Direccion,
                Localidad = c.Localidad,
                CodigoPostal = c.CodigoPostal,
                Provincia = c.Provincia,
                Latitud = c.Latitud,
                Longitud = c.Longitud,
                Telefono = c.Telefono,
                Email = c.Email,
                Web = c.Web,
                Facebook = c.Facebook,
                Twitter = c.Twitter,
                LogotipoDocumentoID = c.LogotipoDocumentoID,
                Descripcion = c.Descripcion,
                Tags = c.Tags,
                LogotipoUrl = c.LogotipoDocumentoID.HasValue ? $"Home/LogotipoEmpresa/{c.EmpresaID}" : "",
                BannerDocumentoID = c.BannerDocumentoID,
                BannerUrl = c.BannerDocumentoID.HasValue ? $"Home/BannerEmpresa/{c.BannerDocumentoID}" : "",
            };

        public long EmpresaID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public long? LogotipoDocumentoID { get; set; }
        public string Descripcion { get; set; }
        public string LogotipoUrl { get; set; }
        public string Tags { get; set; }

        public string BannerUrl { get; set; }
        public long? BannerDocumentoID { get; set; }
    }
}