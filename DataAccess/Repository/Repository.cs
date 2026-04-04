using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel.intrefaces;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        protected DbSet<T> dbSet;

        public Repository(ApplicationDbContext database)
        {
            _db = database;
            dbSet = _db.Set<T>();
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool tracked = false,
            Expression<Func<T, object>>? sortBy = null)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (sortBy != null)
            {
                query = query.OrderBy(sortBy);
            }

            return await query.ToListAsync();
        }

        public async virtual Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null,
            bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public void Remove(T entity)
        {
            if (entity is ISoftDeletable soft)
            {
                soft.DeletedAt = DateTime.UtcNow;
                _db.Update(entity);
            }
            else
            {
                dbSet.Remove(entity);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    if (entity is ISoftDeletable soft)
                    {
                        soft.DeletedAt = DateTime.UtcNow;
                        _db.Update(entity);
                    }
                }
            }
            else
            {
                dbSet.RemoveRange(entities);
            }
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }
    }
}
