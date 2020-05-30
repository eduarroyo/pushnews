using System;
using Entity = PushNews.Dominio.Entidades.Telefono;

namespace PushNews.WebService.Models
{
    public class TelefonoModel
    {
        public static Func<Entity, TelefonoModel> FromEntity =
            c => new TelefonoModel
            {
                TelefonoID = c.TelefonoID,
                Numero = c.Numero,
                Descripcion = c.Descripcion,
                Fecha = c.Fecha,
                Activo = c.Activo
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Numero = Numero.Trim();
            modificar.Descripcion = Descripcion.Trim();
            modificar.Fecha = Fecha;
            modificar.Activo = Activo;
        }
        public long TelefonoID { get; set; }
        public string Numero { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Activo { get; set; }
    }
}