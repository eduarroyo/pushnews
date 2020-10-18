using System;
using Entity = PushNews.Dominio.Entidades.Categoria;

namespace PushNews.WebService.Models
{
    public class CategoriaModel
    {
        public static Func<Entity, CategoriaModel> FromEntity =
            c => new CategoriaModel
            {
                CategoriaID = c.CategoriaID,
                Nombre = c.Nombre,
                Orden = c.Orden,
                Icono = c.Icono,
                Activo = c.Activo
            };

        public void ActualizarEntidad(Entity modificar)
        {
            modificar.Nombre = Nombre.Trim();
            modificar.Icono = Icono;
            modificar.Orden = Orden;
            modificar.Activo = Activo;
        }
        public long CategoriaID { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
        public string Icono { get; set; }
        public bool Activo { get; set; }
    }
}