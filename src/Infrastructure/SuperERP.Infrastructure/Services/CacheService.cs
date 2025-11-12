using System.Text.Json;

namespace SuperERP.Infrastructure.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
}

public class MemoryCacheService : ICacheService
{
    private readonly Dictionary<string, (object Value, DateTime? Expiration)> _cache = new();

    public Task<T?> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out var cached))
        {
            if (cached.Expiration == null || cached.Expiration > DateTime.UtcNow)
            {
                return Task.FromResult((T?)cached.Value);
            }
            _cache.Remove(key);
        }
        return Task.FromResult(default(T));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var expirationTime = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : (DateTime?)null;
        _cache[key] = (value!, expirationTime);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
