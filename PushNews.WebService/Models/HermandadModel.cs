using System;
using Entity = PushNews.Dominio.Entidades.Hermandad;

namespace PushNews.WebService.Models
{
    public class HermandadModel
    {
        public static Func<Entity, HermandadModel> FromEntity =
            c => new HermandadModel
            {
                HermandadID = c.HermandadID,
                Nombre = c.Nombre,
                LogotipoDocumentoID = c.LogotipoDocumentoID,
                LogotipoUrl = c.LogotipoDocumentoID.HasValue ? $"Home/LogotipoHermandad/{c.HermandadID}" : "",
                IglesiaNombre = c.IglesiaNombre,
                IglesiaDireccion = c.IglesiaDireccion,
                IglesiaLatitud = c.IglesiaLatitud,
                IglesiaLongitud = c.IglesiaLongitud,
                Activo = c.Activo
            };

        public long HermandadID { get; set; }

        public string Nombre { get; set; }

        public string IglesiaNombre { get; set; }

        public string IglesiaDireccion { get; set; }

        public double? IglesiaLatitud { get; set; }

        public double? IglesiaLongitud { get; set; }

        public bool Activo { get; set; }

        public long? LogotipoDocumentoID { get; set; }

        public string LogotipoUrl { get; set; }

    }
}
