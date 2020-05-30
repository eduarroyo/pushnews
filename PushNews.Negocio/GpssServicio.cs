using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Gpss;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class GpssServicio : BaseServicio<Gps>, IGpssServicio
    {
        private Aplicacion aplicacion;

        public GpssServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }

            CamposEvitarDuplicados = new[] { "Matricula", "GpsApiID" };
        }

        protected override void ComprobarDuplicados(Gps gps)
        {
            // Buscar un GPS con la misma matrícula y aplicación pero con diferente ID.
            Gps otraMatricula = GetSingle(p => p.Matricula == gps.Matricula
                                         && p.AplicacionID == gps.AplicacionID
                                         && p.GpsID != gps.GpsID);
            if (otraMatricula != null)
            {
                throw new GpsMatriculaExisteException(gps.Matricula);
            }

            // Buscar un GPS con el mismo APIID y aplicación pero con diferente ID.
            Gps otroApiID = GetSingle(p => p.GpsApiID == gps.GpsApiID
                                         && p.AplicacionID == gps.AplicacionID
                                         && p.GpsID != gps.GpsID);

            if (otroApiID != null)
            {
                throw new GpsApiIDExisteException(gps.GpsID);
            }
        }

        public override void Insert(Gps entity)
        {
            if (aplicacion != null)
            {
                entity.AplicacionID = aplicacion.AplicacionID;
            }
            entity.Activo = true;
            base.Insert(entity);
        }
    }
}
