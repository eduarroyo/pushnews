using System;
using System.Threading.Tasks;

namespace PushNews.Dominio.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// Actualiza la base de datos con los cambios. Ante un conflicto en los cambios lanzará 
        /// una excepción.
        /// </summary>
        void Commit();

        Task<int> CommitAsync();

        /// <summary>
        /// Actualiza la base de datos con los cambios. Si hay conflictos actualizará los datos del
        /// cliente ("client wins").
        /// </summary>
        void CommitAndRefreshChanges();

        // Descarta los cambios de la UnitOfWork y que están siendo observados por ella.
        void Rollback();
    }
}
