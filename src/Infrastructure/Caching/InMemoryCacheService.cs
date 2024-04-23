using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiTemplate.Infrastructure.Caching;

public class InMemoryCacheService<TKey, TValue> : ICacheService<TKey, TValue>
{
    private readonly IMemoryCache _cache;
    private readonly JsonSerializerOptions _serializerOptions;

    public InMemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
        _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public TValue Get(TKey key)
    {
        var serializedValue = _cache.Get<string>(key.ToString());
        return serializedValue == null
            ? default
            : JsonSerializer.Deserialize<TValue>(serializedValue, _serializerOptions);
    }

    public void Add(TKey key, TValue value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
    {
        var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
        var memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime, SlidingExpiration = slidingExpirationTime
        };
        _cache.Set(key.ToString(), serializedValue, memoryCacheEntryOptions);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var serializedValue = _cache.Get<string>(key.ToString());
        if (serializedValue != null)
        {
            value = JsonSerializer.Deserialize<TValue>(serializedValue, _serializerOptions);
            return true;
        }

        value = default;
        return false;
    }

    public void Remove(TKey key)
    {
        _cache.Remove(key.ToString());
    }
}
