namespace PushNews.Negocio.Excepciones.Aplicaciones
{
    public class SubdominioExisteException : PushNewsException
    {
        public SubdominioExisteException(string subdominio)
        {
            Subdominio = subdominio;
        }

        public string Subdominio { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Mensajes.SubdominioExisteException, Subdominio);
            }
        }
    }
}