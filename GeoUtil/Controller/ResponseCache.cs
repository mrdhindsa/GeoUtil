using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace GeoUtil.Controller
{
    public class ResponseCache
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _expirationTime;

        public ResponseCache(TimeSpan expirationTime)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _expirationTime = expirationTime;
        }

        public void AddResponse(string location, LocationResponse response)
        {
            if (response == null)
                return;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _expirationTime,
                SlidingExpiration = _expirationTime
            };

            _cache.Set(location, response, cacheEntryOptions);
        }

        public LocationResponse? GetResponse(string location)
        {
            _cache.TryGetValue(location, out LocationResponse? response);
            return response;
        }
    }
}
