using PushNews.Dominio.Entidades;

namespace PushNews.Negocio.Interfaces
{
    public interface IComunicacionesAccesosServicio : IBaseServicio<ComunicacionAcceso>
    {
        void AccesoTerminal(Comunicacion comunicacion, string uid, string ip);
    }
}
