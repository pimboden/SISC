using System;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Common.Runtime.Caching
{
    public interface ICacheHandler
    {
        IObjectCache AirlineCache { get; }
        object AirlineCacheLock { get; }
        TimeSpan AirlineCacheTimeout { get; }

        CacheInfo GetCacheInfo(string cacheName);
        bool ClearCaches(string password);

    }
}
