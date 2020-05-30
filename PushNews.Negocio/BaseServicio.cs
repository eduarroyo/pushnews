using PushNews.Dominio;
using PushNews.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PushNews.Negocio
{
    /// <summary>
    /// Contiene las implementaciones comunes de los métodos de la interfaz IBaseServicio.
    /// Todos los métodos deben poder ser sobrescritos.
    /// </summary>
    /// <typeparam name="TEntity">Clase del tipo de entidad del dominio de PushNews.</typeparam>
    public abstract class BaseServicio<TEntity> : IBaseServicio<TEntity> where TEntity: class
    {
        protected readonly IPushNewsUnitOfWork unitOfWork;

        protected BaseServicio(IPushNewsUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lista de nombres de campos de <see cref="TEntity"/> cuyo valor no puede repetirse en diferentes
        /// registros. Asignar en el constructor de las clases herederas si procede.
        /// Relacionado con los métodos <seealso cref="ComprobarDuplicados"/> y <seealso cref="CheckChanges"/>
        /// </summary>
        protected IEnumerable<string> CamposEvitarDuplicados { get; set; }

        private IQueryable<TEntity> BuildQueryGet(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties)
        {
            IQueryable<TEntity> query = unitOfWork.Set<TEntity>();

            if(RestrictFilter != null)
            {
                query = query.Where(RestrictFilter);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        /// <summary>
        /// Asignar esta propiedad en las clases derivadas nos permite aplicar filtros comunes para todas
        /// las consultas que se realicen desde el objeto de servicio.
        /// Por ejemplo, si se asigna una expresión o => o.AyuntamientoID == X a RestrictFilter obligaremos a
        /// restringir las búsquedas por clínica para todas las consultas que se realicen a través del objeto
        /// de servicio. X sería una expresión de la clase derivada, por ejemplo una propiedad readonly.
        /// </summary>
        protected Expression<Func<TEntity, bool>> RestrictFilter { get; set; } = null;

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = BuildQueryGet(filter, orderBy, includeProperties);
            return query;
        }
        
        public virtual IEnumerable<TEntity> Get(int startIndex, int take,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            if (startIndex < 0) startIndex = 0;
            IQueryable<TEntity> query = BuildQueryGet(filter, orderBy, includeProperties);
            return query.Skip(startIndex).Take(take);
        }

        public async virtual Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = BuildQueryGet(filter, orderBy, includeProperties);
            return await query.ToListAsync();
        }

        public async Task<IList<TEntity>> GetAsync(int startIndex, int take,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            if (startIndex < 0) startIndex = 0;
            IQueryable<TEntity> query = BuildQueryGet(filter, orderBy, includeProperties);
            return await query.Skip(startIndex).Take(take).ToListAsync();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            var preQuery = unitOfWork.Set<TEntity>().AsQueryable();
            if(RestrictFilter != null)
            {
                preQuery = preQuery.Where(RestrictFilter);
            }
            return preQuery.SingleOrDefault(filter);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            var preQuery = unitOfWork.Set<TEntity>().AsQueryable();
            if (RestrictFilter != null)
            {
                preQuery = preQuery.Where(RestrictFilter);
            }
            return await preQuery.SingleOrDefaultAsync(filter);
        }

        // Quitar GetById para garantizar que todas las consultas se realizan a través de 
        // Get o GetSingle o sus versiones asíncronas. Esto es necesario para garantizar
        // que se aplica la restricción de filtros. ver BaseServicio.RestrictFilter
        //public virtual TEntity GetById(params object[] id)
        //{
        //    return unitOfWork.Set<TEntity>().Find(id);
        //}

        public virtual void Insert(TEntity entity)
        {
            if (CamposEvitarDuplicados != null && CamposEvitarDuplicados.Any())
            {
                ComprobarDuplicados(entity);
            }
            unitOfWork.Set<TEntity>().Add(entity);
        }
        
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = unitOfWork.Set<TEntity>().Find(id);
            Delete(entityToDelete);
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entitiesToDelete)
        {
            IDbSet<TEntity> set = unitOfWork.Set<TEntity>();
            IEnumerator<TEntity> it = entitiesToDelete.GetEnumerator();
            while (it.MoveNext())
            {
                Delete(it.Current);
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            unitOfWork.Attach(entityToDelete);
            unitOfWork.Set<TEntity>().Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            unitOfWork.SetModified(entityToUpdate);
        }

        public virtual void ApplyChanges()
        {
            if(CamposEvitarDuplicados != null && CamposEvitarDuplicados.Any())
            {
                CheckChanges();
            }
            unitOfWork.Commit();
        }

        public virtual async Task ApplyChangesAsync()
        {
            if (CamposEvitarDuplicados != null && CamposEvitarDuplicados.Any())
            {
                CheckChanges();
            }
            await unitOfWork.CommitAsync();
        }

        public IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters)
        {
            return unitOfWork.ExecuteQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters)
        {
            return unitOfWork.ExecuteCommand(sqlCommand, parameters);
        }

        public async Task<int> ExecuteInDatabaseByQueryAsync(string sqlCommand, params object[] parameters)
        {
            return await unitOfWork.ExecuteCommandAsync(sqlCommand, parameters);
        }

        public TEntity Create()
        {
            return unitOfWork.Set<TEntity>().Create();
        }

        public void LoadProperty(TEntity entity, string navigationField)
        {
            unitOfWork.LoadProperty(entity, navigationField);
        }

        public async Task LoadPropertyAsync(TEntity entity, string navigationField)
        {
            await unitOfWork.LoadPropertyAsync(entity, navigationField);
        }

        public void LoadCollection(TEntity entity, string navigationField)
        {
            unitOfWork.LoadCollection(entity, navigationField);
        }

        public async Task LoadCollectionAsync(TEntity entity, string navigationField)
        {
            await unitOfWork.LoadCollectionAsync(entity, navigationField);
        }

        /// <summary>
        /// Debe lanzar alguna excepción si existen registros previos y distintos (diferente id) que tengan
        /// el mismo valor que <paramref name="entity"/> para alguno de los campos de la lista
        /// <see cref="CamposEvitarDuplicados"/>.
        /// </summary>
        protected virtual void ComprobarDuplicados(TEntity entity)
        {
            // Implementar en clases derivadas.
        }

        /// TODO Generalizar y llevar este método a la clase base.
        /// La lista de campos a comprobar sería un array protected, asignado en el constructor de las clases
        /// derivadas. Estaría vacío por defecto y eso significa que no hay que comprobar nada.
        /// El método ComprobarCambios sería virtual, implementado por cada clase derivada.
        protected void CheckChanges()
        {
            var changes = unitOfWork.ChangeTracker;
            if (changes.HasChanges())
            {
                // Obtener registros modificados a la espera de ser persistidos.
                var actualizaciones = changes.Entries<TEntity>().Where(ch => ch.State == EntityState.Modified);

                // Para cada registro modificado, comprobar si existe un registro diferente con igual nombre
                // o subdominio, en cuyo caso se lanzará la excepción correspondiente.
                foreach (var actualizacion in actualizaciones)
                {
                    foreach(string campoComprobar in CamposEvitarDuplicados)
                    {
                        object original = actualizacion.OriginalValues.GetValue<object>(campoComprobar);
                        object actual = actualizacion.CurrentValues.GetValue<object>(campoComprobar);
                        if(original != actual)
                        {
                            ComprobarDuplicados(actualizacion.Entity);
                            break;
                        }
                    }
                }
            }
        }
    }
}
