using System;
using System.Runtime.Serialization;

namespace PushNews.Negocio.Excepciones
{
    /// <summary>
    /// Clase base para todas las excepciones de la capa de negocio de BandoApp.
    /// Por el momento no se mantiene ninguna información asociada a la excepción. Cualquier dato
    /// asociado a los errores de dominio se puede almacenar en campos de esta clase de forma que 
    /// puedan ser escritos en el log de error de forma genérica.
    /// 
    /// Podría ser un Dictionary<string,string> y que cada tipo derivado agregue sus claves 
    /// y valores, además de un texto de error.
    /// </summary>
    public class PushNewsException: Exception
    {
        public PushNewsException(): base()
        { }

        public PushNewsException(string message): base(message)
        { }

        public PushNewsException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public PushNewsException(SerializationInfo info, StreamingContext context): base(info, context)
        { }
    }

}
