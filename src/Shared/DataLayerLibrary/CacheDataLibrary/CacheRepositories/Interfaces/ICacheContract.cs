namespace DataCacheLayer.CacheRepositories.Interfaces;

public interface ICacheContract
{
    /// <summary>
    /// Read from cache
    /// </summary>
    /// <returns>Return list of records for LibraryAuthorModel.</returns>
    Task<List<T>?> ReadFromCacheAsync<T>(string key, int _modificationInDays, bool GenerateSpecificKey = false);

    /// <summary>
    /// Add new cache data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="record"></param>
    /// <param name="_expirationTime"></param>
    /// <param name="isNew"></param>
    /// <returns></returns>
    Task<(bool status, string message)> AddCacheAsync<T>(string id, T record, DateTimeOffset _expirationTime);
    /// <summary>
    /// Update record on cache server
    /// </summary>
    /// <param name="id"></param>
    /// <param name="record"></param>
    /// <param name="isNew">true for new record, false for upating record</param>
    /// <returns></returns>
    Task<(bool status, string message)> UpdateCacheAsync<T>(string key, string id, T record, DateTimeOffset _expirationTime, bool isNew = false, bool GenerateSpecificKey = false);

    /// <summary>
    /// Remove record from cache
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool status, string message)> RemoveFromCacheAsync<T>(string key, string id, int _modificationInDays, DateTimeOffset _expirationTime, bool GenerateSpecificKey = false);

    /// <summary>
    /// Generate cache key for get/set data inside cache server
    /// </summary>
    /// <typeparam name="T">Object model, with this name the key get generated</typeparam>
    /// <param name="key">id for that record to generate key</param>
    /// <param name="clientId">School Id</param>
    /// <returns></returns>
    string GenerateKeyForCache<T>(string key, string clientId);
    //string GenerateSpecificKeyForCache(string key);
}