using PushNews.Dominio.Entidades;

namespace PushNews.WebApp.Models.Opciones
{
    public class OpcionesModel
    {
        public OpcionesModel(Usuario emp)
        {
            Cultura = new CulturaModel(emp);
        }

        public CulturaModel Cultura { get; set; }
    }
}