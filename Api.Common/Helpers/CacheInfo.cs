using System;
using Sisc.Api.Common.Runtime.Caching;

namespace Sisc.Api.Common.Helpers
{
    public class CacheInfo
    {
        public CacheInfo()
        {
            
        }

        public CacheInfo(IObjectCache objectCache, object cacheLock, TimeSpan? timeout, CacheType cacheType = CacheType.Absolute)
        {
            ObjectCache = objectCache;
            CacheLock = cacheLock;
            Timeout = timeout ?? new TimeSpan(0,0,1,0);
            Type = cacheType;
        }

        public enum CacheType
        {
            Absolute,
            Sliding
        }
        public IObjectCache ObjectCache { get; set; }
        public object CacheLock { get; set; }
        public TimeSpan Timeout { get; set; }
        public CacheType Type { get; set; }
        public bool HasCache => ObjectCache != null;
    }
}
