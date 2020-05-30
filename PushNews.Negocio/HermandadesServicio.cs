using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class HermandadesServicio : BaseServicio<Hermandad>, IHermandadesServicio
    {
        private Aplicacion aplicacion;

        public HermandadesServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Hermandad entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            entity.Activo = true;
            base.Insert(entity);
        }
    }
}