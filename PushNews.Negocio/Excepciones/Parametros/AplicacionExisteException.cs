namespace PushNews.Negocio.Excepciones.Parametros
{
    public class ParametroExisteException : PushNewsException
    {
        public ParametroExisteException(string nombreParametro)
        {
            NombreParametro = nombreParametro;
            NombreAplicacion = null;
        }
        public ParametroExisteException(string nombreParametro, string nombreAplicacion)
        {
            NombreParametro = nombreParametro;
            NombreAplicacion = nombreAplicacion;
        }

        public string NombreParametro { get; private set; }
        public string NombreAplicacion { get; private set; }

        public override string Message
        {
            get
            {

                return string.IsNullOrEmpty(NombreAplicacion)
                    ? string.Format(Mensajes.ParametroExisteException, NombreParametro)
                    : string.Format(Mensajes.ParametroAplicacionExisteException, NombreParametro, NombreAplicacion);
            }
        }
    }
}