using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Data.Common
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(object[] keys);

        Task<TEntity> GetAsync(object[] keys, CancellationToken cancellationToken);

        List<TEntity> GetAll(BaseQueryParams queryParams);

        Task<List<TEntity>> GetAllAsync(BaseQueryParams queryParams, CancellationToken cancellationToken);

        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, CancellationToken cancellationToken);

        EntityEntry<TEntity> Add(TEntity entity);

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);

        void AddRange(List<TEntity> entities);

        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);

        void Remove(TEntity entity);

        void RemoveRange(List<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(List<TEntity> entities);

        int Complete();

        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}
