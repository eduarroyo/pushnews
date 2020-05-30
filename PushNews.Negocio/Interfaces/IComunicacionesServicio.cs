using PushNews.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PushNews.Negocio.Interfaces
{
    public interface IComunicacionesServicio : IBaseServicio<Comunicacion>
    {
        /// <summary>
        /// Obtiene las comunicaciones publicadas, esto es, visibles para los clientes. Estas comunicaciones
        /// son las que están activas, no están borradas, la categoría a la que pertenecen está activa y
        /// su fecha de publicación es anterior a la fecha actual.
        /// </summary>
        /// <param name="categoriaID">Especificar para limitar las comunicaciones a las pertenecientes a una
        /// determinada categoría.</param>
        /// <param name="soloDestacadas">Verdadero para obtener sólo las comunicaciones destacadas.</param>
        /// <param name="timestamp">Especificar para limitar las comunicaciones a las que tengan un timestamp
        /// posterior al valor. Sirve para obtener sólo las comunicaciones cuya última versión ya se ha
        /// descargado.</param>
        IEnumerable<Comunicacion> Publicadas(long? categoriaID = null, bool soloDestacadas = false, long? timestamp = null, bool incluirPrivadas = false);

        /// <summary>
        /// Obtiene las comunicaciones publicadas, esto es, visibles para los clientes. Estas comunicaciones
        /// son las que están activas, no están borradas, la categoría a la que pertenecen está activa y
        /// su fecha de publicación es anterior a la fecha actual.
        /// </summary>
        /// <param name="categoriaID">Especificar para obtener sólo las comunicaciones a las pertenecientes a
        /// una determinada categoría.</param>
        /// <param name="soloDestacadas">Verdadero para obtener sólo las comunicaciones destacadas.</param>
        /// <param name="timestamp">Especificar para obtener sólo comunicaciones que tengan un timestamp
        /// posterior al valor. Sirve para obtener sólo las comunicaciones cuya última versión ya se ha
        /// descargado.</param>
        Task<IEnumerable<Comunicacion>> PublicadasAsync(long? categoriaID = null, bool soloDestacadas = false, long? timestamp = null, bool incluirPrivadas = false);

        Comunicacion ConsultarComunicacion(long comunicacionID, string uid, string ip, long? asociadoId = null);
    }
}
