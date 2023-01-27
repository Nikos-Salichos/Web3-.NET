using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

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

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = slidingExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options, default);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache,
                                               string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

    }
}
