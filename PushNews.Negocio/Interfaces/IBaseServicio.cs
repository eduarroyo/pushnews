using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PushNews.Negocio.Interfaces
{
    public interface IBaseServicio<TEntity> where TEntity: class
    {
        void ApplyChanges();
        Task ApplyChangesAsync();
        void Delete(TEntity entityToDelete);
        void Delete(object id);
        void DeleteAll(IEnumerable<TEntity> entitiesToDelete);
        int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters);
        Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand, params object[] parameters);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IEnumerable<TEntity> Get(int startIndex, int take, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<IList<TEntity>> GetAsync(int startIndex, int take, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        //TEntity GetById(params object[] id);
        IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters);
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter);
        void Insert(TEntity entity);

        TEntity Create();

        void Update(TEntity entityToUpdate);


        void LoadProperty(TEntity entity, string navigationField);
        Task LoadPropertyAsync(TEntity entity, string navigationField);
        void LoadCollection(TEntity entity, string navigationField);
        Task LoadCollectionAsync(TEntity entity, string navigationField);
    }
}