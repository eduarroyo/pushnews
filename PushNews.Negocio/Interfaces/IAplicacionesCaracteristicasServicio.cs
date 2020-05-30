using PushNews.Dominio.Entidades;

namespace PushNews.Negocio.Interfaces
{
    public interface IAplicacionesServicio: IBaseServicio<Aplicacion>
    {
        Aplicacion GetBySubdomain(string subdomain);
        Aplicacion GetByName(string name);
    }
}