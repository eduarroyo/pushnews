using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Interfaces;
using System;

namespace PushNews.Negocio
{
    public class LocalizacionesServicio : BaseServicio<Localizacion>, ILocalizacionesServicio
    {
        private Aplicacion aplicacion;

        public LocalizacionesServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }
        }

        public override void Insert(Localizacion entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            entity.Fecha = DateTime.Now;
            base.Insert(entity);
        }
    }
}
