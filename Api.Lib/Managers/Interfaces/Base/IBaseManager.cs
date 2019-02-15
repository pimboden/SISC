using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Lib.Managers
{
    public interface IBaseManager<TEntity> where TEntity : class
    {
        TEntity Get(object[] keys);
        Task<TEntity> GetAsync(object[] keys, CancellationToken cancellationToken);

        List<TEntity> GetAll(BaseQueryParams queryParams);
        Task<List<TEntity>> GetAllAsync(BaseQueryParams queryParams, CancellationToken cancellationToken);

        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, CancellationToken cancellationToken);

        int SaveNew(TEntity entity);
        Task<int> SaveNewAsync(TEntity entity, CancellationToken cancellationToken);

        int SaveNew(List<TEntity> entities);
        Task<int> SaveNewAsync(List<TEntity> entities, CancellationToken cancellationToken);

        int Remove(TEntity entity);
        Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken);

        int Remove(List<TEntity> entities);
        Task<int> RemoveAsync(List<TEntity> entities, CancellationToken cancellationToken);

        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        int Update(List<TEntity> entities);
        Task<int> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken);
    }
}
