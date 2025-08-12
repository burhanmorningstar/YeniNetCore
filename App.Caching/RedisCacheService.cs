using App.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace App.Caching;

public class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var bytes = await cache.GetAsync(key, ct);
        return bytes is null ? default : JsonSerializer.Deserialize<T>(bytes, Json);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absolute = null, TimeSpan? sliding = null, CancellationToken ct = default)
    {
        var opts = new DistributedCacheEntryOptions();
        if (absolute is not null) opts.AbsoluteExpirationRelativeToNow = absolute;
        if (sliding is not null) opts.SlidingExpiration = sliding;

        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, Json);
        await cache.SetAsync(key, bytes, opts, ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
        => cache.RemoveAsync(key, ct);
}