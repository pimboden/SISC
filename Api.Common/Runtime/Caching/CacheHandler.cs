using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Common.Runtime.Caching
{
    public class CacheHandler : ICacheHandler
    {
        private readonly IConfiguration _configuration;

        public IObjectCache AirlineCache { get; private set; }
        public object AirlineCacheLock { get; }
        public TimeSpan AirlineCacheTimeout { get; }

        public object ClearCacheLock { get; }

        public CacheHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            ClearCacheLock = new object();

            AirlineCache = new ObjectCache(new MemoryCache(new MemoryCacheOptions()));
            AirlineCacheLock = new object();
            AirlineCacheTimeout = new TimeSpan(1,0,0,0);
        }

        public CacheInfo GetCacheInfo(string cacheName)
        {
            switch (cacheName)
            {
                case CacheName.AirlineCache:
                    return new CacheInfo{ ObjectCache = AirlineCache, CacheLock = AirlineCacheLock, Timeout = AirlineCacheTimeout };
                    break;
                default:
                    return new CacheInfo();
            }
        }
        public bool ClearCaches(string password)
        {
            if (password.Equals(_configuration.GetSection("AppSettings")["ClearCachePassword"],
                StringComparison.InvariantCulture))
            {
                lock (ClearCacheLock)
                {
                    lock (AirlineCacheLock)
                    {
                        AirlineCache = new ObjectCache(new MemoryCache(new MemoryCacheOptions()));
                    }
                }

                return true;
            }

            return false;
        }
    }
}