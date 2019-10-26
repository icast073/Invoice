using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Invoice.Data.Models;
using Invoice.Repository.Filter;
using Invoice.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Invoice.Repository.Implementations
{
    public abstract class BaseRepository<TEntity, TKey, TFilter> : IBaseRepository<TEntity, TKey, TFilter>
       where TEntity : class where TKey : IComparable where TFilter : BaseFilter
    {
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
        protected InvoiceDbContext DbContext { get; set; }

        public BaseRepository(InvoiceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Added)
            {
                DbSet.Add(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Unchanged;
            DbSet.Remove(entity);
        }

        public abstract IEnumerable<TEntity> GetAll(TFilter filter);

        public abstract Task<IEnumerable<TEntity>> GetAllAsync(TFilter filter);

        public IEnumerable<TEntity> GetAllMatching(Expression<Func<TEntity, bool>> predicate, string[] includes = null)
        {
            return GetAllMatchingHelper(predicate, includes).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllMatchingAsync(Expression<Func<TEntity, bool>> predicate, string[] includes = null)
        {
            return await GetAllMatchingHelper(predicate, includes).ToListAsync();
        }

        public TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        private IQueryable<TEntity> GetAllMatchingHelper(Expression<Func<TEntity, bool>> predicate, string[] includes = null)
        {
            var query = DbSet.AsQueryable();

            if (includes != null && EnumerableExtensions.Any(includes))
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(predicate);
        }
    }
}
