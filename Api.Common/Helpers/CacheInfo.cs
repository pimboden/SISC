using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sisc.Api.Common.Runtime.Caching;

namespace Sisc.Api.Common.Helpers
{
    public class CacheInfo
    {
        public CacheInfo()
        {
            
        }

     
        public enum CacheType
        {
            Absolute,
            Sliding
        }

        public IObjectCache ObjectCache { get; set; }
        public ConcurrentDictionary<string, object> CacheLocks { get; set; }
        public TimeSpan Timeout { get; set; }
        public CacheType Type { get; set; }

        public void AddToCache<T>(T objectToCache, string cacheKey) where T : class
        {
            switch (Type)
            {
                case CacheType.Sliding:
                    ObjectCache.AddSliding(objectToCache, cacheKey, Timeout);
                    break;
                default:
                    ObjectCache.AddAbsolute(objectToCache, cacheKey, Timeout);
                    break;
            }
        }
    }
}
