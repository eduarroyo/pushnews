using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Asociados;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class AsociadosServicio: BaseServicio<Asociado>, IAsociadosServicio
    {
        private Aplicacion aplicacion;
        public AsociadosServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion): base(db)
        {
            this.aplicacion = aplicacion;
            CamposEvitarDuplicados = new [] { "Codigo" };

            RestrictFilter = a => !a.Eliminado && a.AplicacionID == this.aplicacion.AplicacionID;
        }

        protected override void ComprobarDuplicados(Asociado asociado)
        {
            // Buscar un asociado no eliminado con el mismo código y diferente ID.
            Asociado otra = GetSingle(p => p.Codigo == asociado.Codigo && !p.Eliminado
                                         && p.AsociadoID != asociado.AsociadoID);
            if (otra != null)
            {
                throw new AsociadoExisteException(asociado.Codigo, aplicacion.Nombre);
            }
        }

        /// <summary>
        /// Marca para insertar un objeto de tipo Asociado, estableciendo previamente la propieda Activo a
        /// true y la aplicación de trabajo.
        /// </summary>
        /// <param name="entity">Objeto para insertar</param>
        public override void Insert(Asociado entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;
            entity.Activo = true;
            base.Insert(entity);
        }

        public override void Delete(Asociado asociado)
        {
            asociado.Eliminado = true;
        }
    }
}