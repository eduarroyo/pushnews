namespace PushNews.Negocio.Excepciones.Aplicaciones
{
    public class AplicacionExisteException : PushNewsException
    {
        public AplicacionExisteException(string nombreAplicacion)
        {
            NombreAplicacion = nombreAplicacion;
        }

        public string NombreAplicacion { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Mensajes.AplicacionExisteException, NombreAplicacion);
            }
        }
    }
}