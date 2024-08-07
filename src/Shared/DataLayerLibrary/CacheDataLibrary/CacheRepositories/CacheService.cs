using GenericFunction;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DataCacheLayer.CacheRepositories
{
    /// <summary>
    /// Default cache service for connection reaper class server
    /// </summary>
    public class CacheService
    {
        private static IDatabase _cacheDatabase;
        public static bool CacheRunning { get; set; } = true;

        static CacheService()
        {
            _cacheDatabase = ConfigureRedis();
        }

        static IDatabase ConfigureRedis()
        {
            try
            {
                return ConnectionHelper.Connection.GetDatabase();
            }
            catch (Exception e)
            {
                e.SendExceptionMailAsync().GetAwaiter().GetResult();
                CacheRunning = false;
            }
            return default;

        }

        /// <summary>
        /// To Read data from cache server
        /// </summary>
        /// <typeparam name="T">Generic Object</typeparam>
        /// <param name="key">Key to find</param>
        /// <returns>return T object if key is available.</returns>

        public static async Task<T?> GetDataAsync<T>(string key)
        {
            CacheRunning = true;
            try
            {

                RedisValue value = await _cacheDatabase.StringGetAsync(key);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }

            }
            catch (Exception e)
            {
                await e.SendExceptionMailAsync();
                CacheRunning = false;
            }
            return default;
        }
        /// <summary>
        /// Set data on redis server
        /// </summary>
        /// <typeparam name="T">Generic Object</typeparam>
        /// <param name="key">Key to find</param>
        /// <param name="value">Generic T Object</param>
        /// <param name="expirationTime">Expiration time for the key</param>
        /// <returns>returns true when object get stored with respective key</returns>
        public static async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            CacheRunning = true;
            try
            {
                TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

                bool isSet = await _cacheDatabase.StringSetAsync(key, value.ToJson(), expiryTime);

                return isSet;

            }
            catch (Exception e)
            {
                await e.SendExceptionMailAsync();

                CacheRunning = false;
            }
            return false;
        }

        /// <summary>
        /// remove data on redis server
        /// </summary>
        /// <param name="key">Key to delete object from cache</param>
        /// <returns>return true when key is deleted.</returns>
        public static async Task<bool> RemoveDataAsync(string key)
        {
            CacheRunning = true;
            try
            {

                bool isKeyExist = await _cacheDatabase.KeyExistsAsync(key);
                if (isKeyExist)
                {
                    return await _cacheDatabase.KeyDeleteAsync(key);
                }

            }
            catch (Exception e)
            {
                await e.SendExceptionMailAsync();
                CacheRunning = false;
            }
            return false;
        }
    }
}
