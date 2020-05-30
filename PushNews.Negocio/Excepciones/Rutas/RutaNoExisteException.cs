namespace PushNews.Negocio.Excepciones.Rutas
{
    public class RutaNoExisteException : PushNewsException
    {
        public RutaNoExisteException(long rutaId)
        {
            RutaID = rutaId;
        }

        public long RutaID { get; private set; }

        public override string Message
        {
            get
            {

                return string.Format(Mensajes.RutaNoExisteException, RutaID);
            }
        }
    }
}