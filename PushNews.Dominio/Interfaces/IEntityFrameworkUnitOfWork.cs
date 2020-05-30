using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PushNews.Dominio.Interfaces
{
    public interface IEntityFrameworkUnitOfWork: IUnitOfWork
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity item) where TEntity : class;
        void SetModified<TEntity>(TEntity item) where TEntity : class;
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters);
    }
}