using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Models.Opciones
{
    public class CulturaModel
    {
        public CulturaModel() { }

        public string Locale { get; set; }
        public CulturaModel(Usuario emp)
        {
            Locale = emp.Locale;
        }
    }
}