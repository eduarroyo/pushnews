namespace PushNews.Negocio.Excepciones.AplicacionesCaracteristicas
{
    public class AplicacionCaracteristicaExisteException : PushNewsException
    {
        public AplicacionCaracteristicaExisteException(string nombreCaracteristica)
        {
            NombreCaracteristica = nombreCaracteristica;
        }

        public string NombreCaracteristica { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Mensajes.AplicacionCaracteristicaExisteException, NombreCaracteristica);
            }
        }
    }
}