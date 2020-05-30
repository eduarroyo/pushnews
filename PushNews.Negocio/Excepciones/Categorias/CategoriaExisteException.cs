namespace PushNews.Negocio.Excepciones.Categorias
{
    public class CategoriaExisteException : PushNewsException
    {
        public CategoriaExisteException(string nombreCategoria, string nombreAplicacion)
        {
            NombreCategoria = nombreCategoria;
            NombreAplicacion = nombreAplicacion;
        }

        public string NombreCategoria { get; private set; }
        public string NombreAplicacion { get; private set; }

        public override string Message
        {
            get
            {

                return string.Format(Mensajes.CategoriaExisteException, NombreCategoria, NombreAplicacion);
            }
        }
    }
}