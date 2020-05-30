using PushNews.Dominio.Entidades;
using System;

namespace PushNews.WebService.Models
{
    public class AplicacionAmigaModel
    {
        public static Func<Aplicacion, AplicacionAmigaModel> FromEntity =
            a => new AplicacionAmigaModel
            {
                Nombre = a.Nombre,
                Subdominio = a.SubDominio
            };

        public string Nombre { get; set; }
        public string Subdominio { get; set; }

    }
}
