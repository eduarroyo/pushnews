using PushNews.Dominio.Entidades;
using System.Collections.Generic;

namespace PushNews.Negocio.Interfaces
{
    public interface ICategoriasServicio : IBaseServicio<Categoria>
    {

        /// <summary>
        /// Modifica el orden de una categoría dado su ID, y renumera las categorías afectadas.
        /// </summary>
        /// <param name="categoriaID">Identificador de la categoría que se va a reordenar.</param>
        /// <param name="nuevoOrden">Nuevo orden que ocupará la categoría.</param>
        void CambiarOrdenCategoria(long categoriaID, int nuevoOrden);

        /// <summary>
        /// Proporciona las categorías activas de la aplicación.
        /// </summary>
        /// <param name="incluirPrivadas">Si es false se obtendrán sólo las categorías activas y públicas.
        /// Si es true, todas las activas.</param>
        IEnumerable<Categoria> ListaCategorias(bool incluirPrivadas = false);
    }
}
