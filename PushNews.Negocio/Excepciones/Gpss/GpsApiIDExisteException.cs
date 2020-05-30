namespace PushNews.Negocio.Excepciones.Gpss
{
    public class GpsApiIDExisteException : PushNewsException
    {
        public GpsApiIDExisteException(long gpsApiId)
        {
            GpsApiID = gpsApiId;
        }

        public long GpsApiID { get; private set; }
        public override string Message
        {
            get
            {

                return string.Format(Mensajes.GpsApiIDExisteException, GpsApiID);
            }
        }

    }
}