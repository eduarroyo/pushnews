using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class RolesServicio : BaseServicio<Rol>, IRolesServicio
    {
        public RolesServicio(IPushNewsUnitOfWork db)
            : base(db)
        {}
    }
}