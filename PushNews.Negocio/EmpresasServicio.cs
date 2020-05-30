using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class EmpresasServicio : BaseServicio<Empresa>, IEmpresasServicio
    {
        private Aplicacion aplicacion;

        public EmpresasServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Empresa entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            base.Insert(entity);
        }
    }
}