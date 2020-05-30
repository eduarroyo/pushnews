namespace PushNews.Negocio.Excepciones.Asociados
{
    public class AsociadoExisteException : PushNewsException
    {
        public AsociadoExisteException(string codigo, string nombreAplicacion)
        {
            Codigo = codigo;
            NombreAplicacion = nombreAplicacion;
        }

        public string Codigo { get; private set; }
        public string NombreAplicacion { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Mensajes.AsociadoExisteException, Codigo, NombreAplicacion);
            }
        }
    }
}