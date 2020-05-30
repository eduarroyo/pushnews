using System;
using Entity = PushNews.Dominio.Entidades.Localizacion;

namespace PushNews.WebService.Models
{
    public class LocalizacionModel
    {
        public static Func<Entity, LocalizacionModel> FromEntity =
            c => new LocalizacionModel
            {
                LocalizacionID = c.LocalizacionID,
                Longitud = c.Longitud,
                Latitud = c.Latitud,
                Descripcion = c.Descripcion,
                Fecha = c.Fecha,
                Activo = c.Activo
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Longitud = Longitud;
            modificar.Latitud = Latitud;
            modificar.Descripcion = Descripcion.Trim();
            modificar.Fecha = Fecha;
            modificar.Activo = Activo;
        }
        public long LocalizacionID { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Activo { get; set; }
    }
}