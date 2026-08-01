using APIEnterprise.Services.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace APIEnterprise.Services;

public class CacheService(HybridCache cache) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan expiration)
    {
        return await cache.GetOrCreateAsync(key, factory,
            options: new HybridCacheEntryOptions
        {
            Expiration = expiration
        });
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}