using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;

namespace PushNews.Negocio
{
    public class TelefonosServicio : BaseServicio<Telefono>, ITelefonosServicio
    {
        private Aplicacion aplicacion;

        public TelefonosServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Telefono entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            entity.Fecha = DateTime.Now;
            base.Insert(entity);
        }
    }
}
