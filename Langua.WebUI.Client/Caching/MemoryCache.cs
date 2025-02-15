using Microsoft.Extensions.Caching.Memory;

namespace Langua.WebUI.Client.Caching
{
    public class MemoryCache<T> 
    {
        private IMemoryCache _memoryCache;
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(10));

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void AddToCache(string Key, T data)
        {
           
            _memoryCache.Set(Key, data,options);
        }
        public void GetDataFromCache(string Key)
        {
            var r = _memoryCache.Get(Key);
            var res = _memoryCache.Get<T>(Key);

        }
    }
}
