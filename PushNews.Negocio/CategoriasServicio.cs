using PushNews.Dominio;
using PushNews.Dominio.Entidades;
using PushNews.Negocio.Excepciones.Categorias;
using PushNews.Negocio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PushNews.Negocio
{
    public class CategoriasServicio : BaseServicio<Categoria>, ICategoriasServicio
    {
        private Aplicacion aplicacion;

        public CategoriasServicio(IPushNewsUnitOfWork db, Aplicacion aplicacion)
            : base(db)
        {
            this.aplicacion = aplicacion;
            CamposEvitarDuplicados = new[] { "Nombre" };
            if (this.aplicacion != null)
            {
                RestrictFilter = c => c.AplicacionID == aplicacion.AplicacionID;
            }
        }

        protected override void ComprobarDuplicados(Categoria categoria)
        {
            // Buscar una categoría con el mismo nombre, la misma aplicación y diferente ID.
            Categoria otra = GetSingle(p => p.Nombre == categoria.Nombre
                                         && p.AplicacionID == categoria.AplicacionID
                                         && p.CategoriaID != categoria.CategoriaID);
            if (otra != null)
            {
                throw new CategoriaExisteException(categoria.Nombre, otra.Aplicacion.Nombre);
            }
        }

        /// <summary>
        /// Inserta una nueva categoría asignando la aplicación.
        /// Incrementa el orden de las categorías de orden igual o superior a la insertada.
        /// </summary>
        public override void Insert(Categoria entity)
        {
            entity.AplicacionID = aplicacion.AplicacionID;

            // Obtener las categorías de orden igual o superior al de entity e incrementar su campo Orden.
            IEnumerable<Categoria> categoriasDesplazar = Get(c => c.Orden >= entity.Orden);
            foreach (var cat in categoriasDesplazar)
            {
                cat.Orden++;
            }

            base.Insert(entity);
        }

        /// <summary>
        /// Modifica el orden de una categoría dado su ID, y renumera las categorías afectadas.
        /// </summary>
        /// <param name="categoriaID">Identificador de la categoría que se va a reordenar.</param>
        /// <param name="nuevoOrden">Nuevo orden que ocupará la categoría.</param>
        public void CambiarOrdenCategoria(long categoriaID, int nuevoOrden)
        {
            // Lista de categorías afectadas por el cambio de orden.
            IEnumerable<Categoria> categoriasModificar;
            // Incremento que se aplicará al orden de las categorías de categoriasModificar.
            int incremento;

            // Categoría que se reubica.
            Categoria original = GetSingle(c => c.CategoriaID == categoriaID);

            // Si el orden nuevo es menor que el original, hay que reubicar las categorías
            // cuyo orden es menor que el original y mayor o igual que el nuevo y sumarles una unidad.
            if (nuevoOrden < original.Orden)
            {
                categoriasModificar = Get(c => c.Orden < original.Orden && c.Orden >= nuevoOrden);
                incremento = 1;
            }

            // Si el nuevo orden mayor que el original, hay que reubicar las categorías
            // cuyo orden es menor que el original y mayor o igual que el nuevo y sumarles una unidad.
            else if (nuevoOrden > original.Orden)
            {
                categoriasModificar = Get(c => c.Orden > original.Orden && c.Orden <= nuevoOrden);
                incremento = -1;
            }

            // Si el nuevo orden es igual que el original, no se hace nada
            else
            {
                categoriasModificar = new List<Categoria>(0);
                incremento = 0;
            }

            // Aplicar el incremento a las categorías afectadas
            foreach (Categoria c in categoriasModificar)
            {
                c.Orden += incremento;
            }

            // Asignar el nuevo orden a la categoría que se ha desplazado.
            original.Orden = nuevoOrden;
        }


        public IEnumerable<Categoria> ListaCategorias(bool incluirPrivadas = false)
        {
            return base
                .Get(c => c.Activo && (!c.Privada || incluirPrivadas))
                .OrderBy(c => c.Orden);
        }
    }
}