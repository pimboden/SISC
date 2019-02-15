using System;
using System.Collections.Concurrent;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Common.Runtime.Caching
{
    public interface ICacheHandler
    {

        bool ClearCaches(string password);

        string GetCacheKey(params object[] cacheParts);

        CacheInfo GetCacheInfo(string cacheName);
    }
}
