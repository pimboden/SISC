using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common.Base;
using Sisc.Api.Common.Helpers;
using Sisc.Api.Data.Common;

namespace Sisc.Api.Lib.Managers.Base
{
    public abstract class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : DataEntity
    {
        private readonly IRepository<TEntity> _repository;


        protected  BaseManager(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual TEntity Get(object[] keys)
        {
             return _repository.Get(keys);
        }

        public virtual async Task<TEntity> GetAsync(object[] keys, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(keys, cancellationToken);
        }

        public virtual List<TEntity> GetAll(BaseQueryParams queryParams)
        {
            return  _repository.GetAll(queryParams);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(BaseQueryParams queryParams, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(queryParams, cancellationToken);
        }

        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            return _repository.Find(predicate, pageIndex, pageSize).ToList();
        }

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return await _repository.FindAsync(predicate, pageIndex, pageSize, cancellationToken);
        }

        public virtual int SaveNew(TEntity entity)
        {
            _repository.Add(entity);
            return _repository.Complete();
            
        }

        public virtual async Task<int> SaveNewAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(entity, cancellationToken);
            return await _repository.CompleteAsync(cancellationToken);
            
        }

        public virtual int SaveNew(List<TEntity> entities)
        {
            _repository.AddRange(entities);
            return _repository.Complete();
           
        }

        public virtual async Task<int> SaveNewAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await _repository.AddRangeAsync(entities, cancellationToken);
            return await _repository.CompleteAsync(cancellationToken);
            
        }

        public virtual int Remove(TEntity entity)
        {
            _repository.Remove(entity);
            return _repository.Complete();
           
        }

        public virtual async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _repository.Remove(entity);
            return await _repository.CompleteAsync(cancellationToken);
           
        }

        public virtual int Remove(List<TEntity> entities)
        {
            _repository.RemoveRange(entities);
            return _repository.Complete();
            
        }

        public virtual  async Task<int> RemoveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            _repository.RemoveRange(entities);
            return await _repository.CompleteAsync(cancellationToken);
            
        }


        public virtual int Update(TEntity entity)
        {
            _repository.Update(entity);
            return _repository.Complete();
          
        }

        public virtual async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _repository.Update(entity);
            return await _repository.CompleteAsync(cancellationToken);
           
        }

        public virtual int Update(List<TEntity> entities)
        {
            _repository.UpdateRange(entities);
            return _repository.Complete();
            
        }

        public virtual async Task<int> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            _repository.UpdateRange(entities);
            return await _repository.CompleteAsync(cancellationToken);
        }
    }
}