using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Data.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        public DbContext Context => _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public TEntity Get(object[] keys)
        {
            return _context.Set<TEntity>().Find(keys);
        }
        public async Task<TEntity> GetAsync(object[] keys, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().FindAsync(keys,cancellationToken);
        }

        public List<TEntity> GetAll(BaseQueryParams queryParams)
        {

          return _context.Set<TEntity>().OrderByColumns(queryParams.OrderByColumns).Skip(queryParams.PageIndex * queryParams.PageSize).Take(queryParams.PageSize).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync(BaseQueryParams queryParams, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().OrderByColumns(queryParams.OrderByColumns)
                .Skip(queryParams.PageIndex * queryParams.PageSize)
                .Take(queryParams.PageSize).ToListAsync(cancellationToken);
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            return _context.Set<TEntity>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity);
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public void AddRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync( cancellationToken);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }

}
