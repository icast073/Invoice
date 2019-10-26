using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Invoice.Repository.Interfaces
{
    public interface IBaseRepository<TEntity, TKey, TFilter>
    {
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        void Add(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll(TFilter filter);
        Task<IEnumerable<TEntity>> GetAllAsync(TFilter filter);
        IEnumerable<TEntity> GetAllMatching(Expression<Func<TEntity, bool>> predicate, string[] includes = null);
        Task<IEnumerable<TEntity>> GetAllMatchingAsync(Expression<Func<TEntity, bool>> predicate, string[] includes = null);
    }
}
