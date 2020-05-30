using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class DocumentosServicio: BaseServicio<Documento>, IDocumentosServicio
    {
        private Aplicacion aplicacion;

        public DocumentosServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion): base(db)
        {
            this.aplicacion = aplicacion;
            if(aplicacion != null)
            {
                RestrictFilter = d => d.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Documento entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            base.Insert(entity);
        }
    }
}
