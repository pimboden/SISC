using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Common.Runtime.Caching
{
    public class CacheHandler : ICacheHandler
    {

        private readonly IConfiguration _configuration;

        private IObjectCache CountriesCache { get; set; }
        private ConcurrentDictionary<string,object> CountriesCacheLocks { get; }
        private TimeSpan CountriesCacheTimeout { get; }

        private object ClearCacheLock { get; }

        public CacheHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            ClearCacheLock = new object();

            CountriesCache = new ObjectCache(new MemoryCache(new MemoryCacheOptions()));
            CountriesCacheLocks = new ConcurrentDictionary<string, object>();
            CountriesCacheTimeout = new TimeSpan(1,0,0,0);
        }


        public string GetCacheKey(params object[] cacheParts)
        {
            var returnValue = string.Empty;
            foreach (var cachePart in cacheParts)
            {
                returnValue += "_" + Convert.ToString(cachePart);
            }

            return returnValue;
        }

        public CacheInfo GetCacheInfo(string cacheName)
        {
            switch (cacheName)
            {
                case CacheName.CountriesCache:
                {
                    return  new CacheInfo
                    {
                        CacheLocks = CountriesCacheLocks,
                        ObjectCache = CountriesCache,
                        Timeout = CountriesCacheTimeout,
                        Type = CacheInfo.CacheType.Sliding
                    };
                }
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
                    lock (CountriesCacheLocks.GetOrAdd(CacheName.CountriesCache, new object()))
                    {
                        CountriesCache = new ObjectCache(new MemoryCache(new MemoryCacheOptions()));
                    }
                }

                return true;
            }

            return false;
        }
    }
}