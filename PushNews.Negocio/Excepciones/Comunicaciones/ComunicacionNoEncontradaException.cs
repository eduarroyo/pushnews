using PushNews.Dominio.Entidades;

namespace PushNews.Negocio.Excepciones.Comunicaciones
{
    public class ComunicacionNoEncontradaException: PushNewsException
    {
        public ComunicacionNoEncontradaException(long comunicacionID, Aplicacion aplicacion)
        {
            NombreAplicacion = aplicacion.Nombre;
            AplicacionID = aplicacion.AplicacionID;
            ComunicacionID = comunicacionID;
        }

        public string NombreAplicacion { get; private set; }
        public long AplicacionID { get; private set; }
        public long ComunicacionID { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(Mensajes.ComunicacionNoEncontradaException,
                    NombreAplicacion, AplicacionID, ComunicacionID);
            }
        }
    }
}
