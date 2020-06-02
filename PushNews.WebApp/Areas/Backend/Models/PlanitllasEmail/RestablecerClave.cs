using System.Collections.Generic;

namespace PushNews.WebApp.Areas.Backend.Models.PlantillasEmail
{
    public class RestablecerClave
    {
        public RestablecerClave(string urlRestablecer, string nombre, string rutaLogo)
        {
            Nombre = nombre;
            Url = urlRestablecer;
            RutaLogo = rutaLogo;
        }

        public string Nombre { get; set; }
        public string Url { get; set; }
        public string RutaLogo { get; set; }
    }
}
