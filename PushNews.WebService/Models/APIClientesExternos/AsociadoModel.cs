using System;
using Entity = PushNews.Dominio.Entidades.Asociado;

namespace PushNews.WebService.Models.ApiClientesExternos
{

    public class AsociadoResponseModel
    {
        public static readonly Func<Entity, AsociadoResponseModel> FromEntity =
            c => new AsociadoResponseModel()
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

        public long AsociadoID { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Direccion { get; set; }

        public string Localidad { get; set; }

        public string CodigoPostal { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Provincia { get; set; }

        public string Observaciones { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
    }
}