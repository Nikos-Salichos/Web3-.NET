using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Persistence.Cache
{
    public static class RedisCacheService
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
                                          string recordId,
                                          T data,
                                          TimeSpan? absoluteExpireTime = null,
                                          TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
        }

    }
}
