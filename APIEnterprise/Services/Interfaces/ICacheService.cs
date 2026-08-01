namespace APIEnterprise.Services.Interfaces;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan expiration);

    Task RemoveAsync(string key);
}