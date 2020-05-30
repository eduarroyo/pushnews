using System.Collections.Generic;

namespace PushNews.WebApp.Models.UI
{
    public class CabeceraModel
    {
        public CabeceraModel()
        {
            ListaOpConfiguracion = new List<EnlaceConfiguracion>(capacity: 0);
        }

        public class EnlaceConfiguracion
        {
            public string Url { get; set; }
            public string Texto { get; set; }
        }

        public string Icono { get; set; }
        public string BCSeccion {get; set;}
        public string UrlSeccion { get; set; }
        public string BCModulo {get; set;}
        public string Titulo {get; set;}
        public string Subtitulo {get; set;}
        public bool Recargar {get;set;}
        public IEnumerable<EnlaceConfiguracion> ListaOpConfiguracion { get; set; }

        public IEnumerable<BotonGridModel> BotonesExtra { get; set; }
    }
}