using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sisc.Api.Common.Base;
using Sisc.Api.Common.Helpers;
using Sisc.Api.Data.Common;

namespace Sisc.Api.Lib.Managers.Base
{
    public abstract class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : DataEntity
    {
        protected readonly CacheInfo CacheInfo;
        private readonly IRepository<TEntity> _repository;

        private readonly IEnumerable<PropertyInfo> _keyProps;

        protected  BaseManager(IRepository<TEntity> repository)
        {
            _repository = repository;
            CacheInfo = new CacheInfo();
        }

        protected BaseManager(IRepository<TEntity> repository, CacheInfo cacheInfo)
        {
            _repository = repository;
            CacheInfo = cacheInfo;
            _keyProps = typeof(TEntity).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).ToList();
        }

        public TEntity Get(object[] keys)
        {
            if (!CacheInfo.HasCache)
            {
                return _repository.Get(keys);
            }

            var itemCacheKey = GetCacheKey(keys);
            var tEntity = TryGetFromCache(itemCacheKey);
            if (tEntity == null)
            {
                tEntity = _repository.Get(keys);
            }

            TryAddToCache(tEntity, itemCacheKey);
            return tEntity;
        }

        public async Task<TEntity> GetAsync(object[] keys, CancellationToken cancellationToken)
        {
            if (!CacheInfo.HasCache)
            {
                return await _repository.GetAsync(keys, cancellationToken);
            }

            var itemCacheKey = GetCacheKey(keys);
            var tEntity = TryGetFromCache(itemCacheKey);
            if (tEntity == null)
            {
                tEntity = await _repository.GetAsync(keys, cancellationToken);
            }

            TryAddToCache(tEntity, itemCacheKey);
            return tEntity;
        }

        public List<TEntity> GetAll(BaseQueryParams queryParams)
        {
            if (!CacheInfo.HasCache)
            {
                return _repository.GetAll(queryParams);
            }

            var allFound = _repository.GetAll(queryParams);

           TryAddToCache(allFound, GetCacheKey(queryParams.OrderByColumns, queryParams.PageIndex, queryParams.PageSize));

            return allFound;
        }

        public async Task<List<TEntity>> GetAllAsync(BaseQueryParams queryParams, CancellationToken cancellationToken)
        {
            if (!CacheInfo.HasCache)
            {
                return await _repository.GetAllAsync(queryParams, cancellationToken);
            }

            var allFound = await _repository.GetAllAsync(queryParams, cancellationToken);

            TryAddToCache(allFound, GetCacheKey(queryParams.OrderByColumns, queryParams.PageIndex, queryParams.PageSize));

            return allFound;
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            return _repository.Find(predicate, pageIndex, pageSize).ToList();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            return await _repository.FindAsync(predicate, pageIndex, pageSize, cancellationToken);
        }

        public int SaveNew(TEntity entity)
        {
            _repository.Add(entity);
            var returnValue = _repository.Complete();
            if (CacheInfo.HasCache)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        public async Task<int> SaveNewAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(entity, cancellationToken);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (CacheInfo.HasCache)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        public int SaveNew(List<TEntity> entities)
        {
            _repository.AddRange(entities);
            var returnValue = _repository.Complete();
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        public async Task<int> SaveNewAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await _repository.AddRangeAsync(entities, cancellationToken);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        public int Remove(TEntity entity)
        {
            _repository.Remove(entity);
            var returnValue = _repository.Complete();
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            TryRemoveCache(GetCacheKey(entity));
            return returnValue;
        }

        public async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _repository.Remove(entity);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            TryRemoveCache(GetCacheKey(entity));
            return returnValue;
        }

        public int Remove(List<TEntity> entities)
        {
            _repository.RemoveRange(entities);
            var returnValue = _repository.Complete();
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryRemoveCache(GetCacheKey(entity));
            }

            return returnValue;
        }

        public async Task<int> RemoveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            _repository.RemoveRange(entities);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryRemoveCache(GetCacheKey(entity));
            }

            return returnValue;
        }

        public int Update(TEntity entity)
        {
            _repository.Update(entity);
            var returnValue = _repository.Complete();
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            TryAddToCache(entity, GetCacheKey(entity));
            return returnValue;
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _repository.Update(entity);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            TryAddToCache(entity, GetCacheKey(entity));
            return returnValue;
        }

        public int Update(List<TEntity> entities)
        {
            _repository.UpdateRange(entities);
            var returnValue = _repository.Complete();
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        public async Task<int> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            _repository.UpdateRange(entities);
            var returnValue = await _repository.CompleteAsync(cancellationToken);
            if (!CacheInfo.HasCache)
            {
                return returnValue;
            }

            foreach (var entity in entities)
            {
                TryAddToCache(entity, GetCacheKey(entity));
            }

            return returnValue;
        }

        protected string GetCacheKey(object[] keys)
        {
            var cacheKey = typeof(TEntity).ToString();
            return keys.Aggregate(cacheKey, (current, keyValue) => $"{current}_{keyValue}");
        }

        protected string GetCacheKey(TEntity instance)
        {
            var cacheKey = typeof(TEntity).ToString();
            string result = cacheKey;
            foreach (var keyProp in _keyProps)
            {
                var o = keyProp.GetValue(instance);
                result = $"{result}_{o}";
            }

            return result;
        }

        protected string GetCacheKey(List<OrderByColumn>orderByColumns, int pageIndex, int pageSize)
        {
            var cacheKey = typeof(TEntity).ToString();
            string result = cacheKey;
            result = $"{result}_{pageIndex* pageSize}_{pageIndex * pageSize + pageSize - 1}";
            if (orderByColumns != null)
            {
                foreach (var orderByColumn in orderByColumns)
                {
                    result = $"{result}_{orderByColumn.ColumnName}_{orderByColumn.Descending}";
                }
            }
            return result;
        }

        protected TEntity TryGetFromCache(string itemCacheKey)
        {
            try
            {
                 return CacheInfo.ObjectCache.Get<TEntity>(itemCacheKey);
            }
            catch
            {
                return default(TEntity);
            }
        }

        protected void TryAddToCache(TEntity tEntity, string itemCacheKey)
        {
            try
            {
                lock (CacheInfo.CacheLock)
                {
                    CacheInfo.ObjectCache.Clear(itemCacheKey);
                    if (CacheInfo.Type == CacheInfo.CacheType.Absolute)
                    {
                        CacheInfo.ObjectCache.AddAbsolute(tEntity, itemCacheKey, CacheInfo.Timeout);
                    }
                    else
                    {
                        CacheInfo.ObjectCache.AddSliding(tEntity, itemCacheKey, CacheInfo.Timeout);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
        protected void TryAddToCache(List<TEntity> tEntities, string itemCacheKey)
        {
            try
            {
                lock (CacheInfo.CacheLock)
                {
                    CacheInfo.ObjectCache.Clear(itemCacheKey);
                    if (CacheInfo.Type == CacheInfo.CacheType.Absolute)
                    {
                        CacheInfo.ObjectCache.AddAbsolute(tEntities, itemCacheKey, CacheInfo.Timeout);
                    }
                    else
                    {
                        CacheInfo.ObjectCache.AddSliding(tEntities, itemCacheKey, CacheInfo.Timeout);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }


        protected void TryRemoveCache(string itemCacheKey)
        {
            try
            {
                lock (CacheInfo.CacheLock)
                {
                    CacheInfo.ObjectCache.Clear(itemCacheKey);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}