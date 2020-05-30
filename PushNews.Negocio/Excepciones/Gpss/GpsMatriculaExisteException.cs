namespace PushNews.Negocio.Excepciones.Gpss
{
    public class GpsMatriculaExisteException : PushNewsException
    {
        public GpsMatriculaExisteException(string matricula)
        {
            Matricula = matricula;
        }

        public string Matricula { get; private set; }

        public override string Message
        {
            get
            {

                return string.Format(Mensajes.GpsMatriculaExisteException, Matricula);
            }
        }
    }
}