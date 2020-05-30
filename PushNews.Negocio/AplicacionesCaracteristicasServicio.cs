using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.AplicacionesCaracteristicas;
using PushNews.Negocio.Interfaces;

namespace PushNews.Negocio
{
    public class AplicacionesCaracteristicasServicio: BaseServicio<AplicacionCaracteristica>, IAplicacionesCaracteristicasServicio
    {
        public AplicacionesCaracteristicasServicio(IPushNewsUnitOfWork db): base(db)
        {
            CamposEvitarDuplicados = new [] { "Nombre" };
        }

        protected override void ComprobarDuplicados(AplicacionCaracteristica caracteristica)
        {
            // Buscar una caracteristica con el mismo nombre y diferente ID.
            AplicacionCaracteristica otra = GetSingle(p => p.Nombre == caracteristica.Nombre
                                         && p.AplicacionCaracteristicaID != caracteristica.AplicacionCaracteristicaID);
            if (otra != null)
            {
                throw new AplicacionCaracteristicaExisteException(caracteristica.Nombre);
            }
        }
    }
}
