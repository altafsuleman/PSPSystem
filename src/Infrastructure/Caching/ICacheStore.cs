namespace WebApiTemplate.Infrastructure.Caching;

public interface ICacheService<TKey, TValue>
{
    /// <summary>
    ///     Gets an item from the cache.
    /// </summary>
    /// <param name="key">The key for the cache item.</param>
    /// <returns>The cached item, or default value if not found.</returns>
    TValue Get(TKey key);

    /// <summary>
    ///     Adds or updates an item in the cache with optional expiration settings.
    /// </summary>
    /// <param name="key">The key for the cache item.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="absoluteExpireTime">The absolute expiration time relative to now.</param>
    /// <param name="slidingExpirationTime">The sliding expiration time.</param>
    void Add(TKey key, TValue value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null);

    /// <summary>
    ///     Attempts to retrieve an item from the cache.
    /// </summary>
    /// <param name="key">The key for the cache item.</param>
    /// <param name="value">The retrieved value. It returns default if not found.</param>
    /// <returns>True if the item exists in the cache, otherwise false.</returns>
    bool TryGetValue(TKey key, out TValue value);

    /// <summary>
    ///     Removes an item from the cache.
    /// </summary>
    /// <param name="key">The key for the cache item to be removed.</param>
    void Remove(TKey key);
}
