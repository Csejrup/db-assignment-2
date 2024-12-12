using Newtonsoft.Json;
using StackExchange.Redis;

namespace Onlinebookstore.Redis;

public class RedisCacheService
{
    private readonly ConnectionMultiplexer _redisConnection;
    private readonly IDatabase _database;

    public RedisCacheService(string connectionString)
    {
        // Connect to Redis server
        _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        _database = _redisConnection.GetDatabase();
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var jsonValue = JsonConvert.SerializeObject(value);

        await _database.StringSetAsync($"{nameof(T)}:${key}", jsonValue, expiry);
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var jsonValue = await _database.StringGetAsync($"{nameof(T)}:${key}");

        if (jsonValue.IsNullOrEmpty)
        {
            return default; // Return the default value if the cache is empty
        }

        return JsonConvert.DeserializeObject<T>(jsonValue);
    }

    public async Task DeleteCacheAsync<T>(string key)
    {
        await _database.KeyDeleteAsync($"{nameof(T)}:${key}");
    }

}