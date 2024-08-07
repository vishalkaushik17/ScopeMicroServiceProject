using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.RequestNResponse.Accounts;
using static GenericFunction.CommonMessages;
namespace DataCacheLayer.CacheRepositories.Repositories
{
    public sealed class CacheRepositoryService : ICacheContract
    {
        private readonly string? _clientId;
        private readonly string? _userId;
        private readonly string? _sessionId;
        private readonly string? _scopeId;
        private readonly DateTimeOffset _expirationTime;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITrace _trace;
        private readonly bool _isTracingRequired;
        private string _cacheKey = string.Empty;

        public CacheRepositoryService(IHttpContextAccessor httpContextAccessor, ITrace trace)
        {
            _userId = UserIdentity.GetUserId(httpContextAccessor.HttpContext);
            _clientId = UserIdentity.GetClientId(httpContextAccessor.HttpContext);
            _sessionId = UserIdentity.GetSessionId(httpContextAccessor.HttpContext);
            _scopeId = UserIdentity.GetScopeId(httpContextAccessor?.HttpContext);
            _expirationTime.AddDays(1);
            _httpContextAccessor = httpContextAccessor;
            _isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
            _trace = trace;

        }
        /// <summary>
        /// Read from cache
        /// </summary>
        /// <returns>Return list of records for LibraryAuthorModel.</returns>
        public async Task<List<T>?> ReadFromCacheAsync<T>(string key, int _modificationInDays, bool GenerateSpecificKey = false)
        {
            List<T>? modelRecords = (List<T>?)Activator.CreateInstance(typeof(List<T>));

            try
            {
                _cacheKey = GenerateSpecificKey ? key : GenerateKeyForCache<T>(key, _clientId);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - {_cacheKey.MarkProcess()} Reading key from cache!");


                modelRecords = await CacheService.GetDataAsync<List<T>>(_cacheKey);
                if (GenerateSpecificKey)
                {
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End  - Reading completed from cache and {modelRecords?.Count ?? 0} records found!");
                    return modelRecords;
                }


                modelRecords?
                .OfType<BaseTemplate>()
                .Where(record => record.CreatedOn <= DateTime.Now && record.CreatedOn >= DateTime.Now.AddDays(-_modificationInDays))
                .ToList()
                .ForEach(record => record.IsEditable = true);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End  - Reading completed from cache and {modelRecords?.Count ?? 0} records found!");
                return modelRecords;

            }
            catch (RuntimeBinderException e)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End - Exception occurred : {e.Message}!");
                await e.SendExceptionMailAsync();
                return modelRecords;

            }
            catch (Exception e)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End - Exception occurred : {e.Message}!");
                await e.SendExceptionMailAsync();
            }
            return default;
        }

        /// <summary>
        /// Update record on cache
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <param name="isNew">true for new record, false for upating record</param>
        /// <returns></returns>
        public async Task<(bool status, string message)> AddCacheAsync<T>(string id, T record, DateTimeOffset _expirationTime)
        {

            try
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - {id.MarkProcess()} Adding key on cache!");

                await CacheService.SetDataAsync(id, record, _expirationTime);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End - {id.MarkProcess()} Key added on cache!");
                return (true, Status.Success);
            }
            catch (Exception e)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End - {id.MarkProcess()} Exception occurred{e.Message}!");
                await e.SendExceptionMailAsync();
                return (false, e.Message);
            }
        }

        /// <summary>
        /// Update record on cache
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <param name="isNew">true for new record, false for upating record</param>
        /// <returns></returns>
        public async Task<(bool status, string message)> UpdateCacheAsync<T>(string key, string id, T record, DateTimeOffset _expirationTime, bool isNew = false,
            bool GenerateSpecificKey = false)
        {

            try
            {

                _cacheKey = GenerateSpecificKey ? GenerateSpecificKeyForCache(key) : GenerateKeyForCache<T>(key, _clientId);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Cache key {_cacheKey.MarkProcess()} Finding key from cache server!");
                List<T>? modelRecords = await CacheService.GetDataAsync<List<T>>(GenerateSpecificKey ? GenerateSpecificKeyForCache(key) : GenerateKeyForCache<T>(key, _clientId));

                if (modelRecords == null)
                {
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Cache key {_cacheKey.MarkProcess()} records not available in cache to update!");
                    return (false, Status.Failed);
                }
                //If record is passed as new - then add to list
                if (isNew)
                {
                    if (string.IsNullOrWhiteSpace((record as BaseTemplate).Id))
                    {
                        return (false, Status.Failed);
                    }
                    modelRecords?.Add(record);
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Record not found for update, Adding record {_cacheKey.MarkProcess()} in cache !");
                }
                else
                {
                    //get record from list which is going to update in cache.
                    T? recordToUpdate = (T?)Convert.ChangeType(modelRecords?.OfType<BaseTemplate>().FirstOrDefault(t => t.Id == id), typeof(T));

                    if (recordToUpdate != null)
                    {
                        //getting index of record which is going to update in list.
                        int index = modelRecords.IndexOf(recordToUpdate);
                        //setting newly updated record on index of list.
                        modelRecords[index] = record;
                    }

                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Updating record {_cacheKey.MarkProcess()} in cache !");
                }

                //updating currency added/update record in cache as list
                await CacheService.SetDataAsync(GenerateKeyForCache<T>(key, _clientId), modelRecords, _expirationTime);

                //return (true, Status.Success);

            }
            catch (Exception e)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Cache key {_cacheKey.MarkProcess()}  Exception occurred{e.Message}!");
                await e.SendExceptionMailAsync();
                return (false, e.Message);
            }
            return (true, Status.Success);
        }

        /// <summary>
        /// Remove record from cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool status, string message)> RemoveFromCacheAsync<T>(string key, string id, int _modificationInDays, DateTimeOffset _expirationTime, bool GenerateSpecificKey = false)
        {
            try
            {

                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - {_cacheKey.MarkProcess()} Removing key from cache!");
                //Read record from cache
                List<T>? modelRecords = await ReadFromCacheAsync<T>(key, _modificationInDays);

                //get record from list which is going to update in cache.
                T? recordToDelete = (T?)Convert.ChangeType(modelRecords?.OfType<BaseTemplate>().FirstOrDefault(t => t.Id == id), typeof(T));

                if (recordToDelete != null)
                {
                    //getting index of record which is going to update in list.
                    int index = modelRecords.IndexOf(recordToDelete);
                    //setting newly updated record on index of list.
                    modelRecords.RemoveAt(index);
                }


                if (modelRecords == null)
                {
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Record {_cacheKey.MarkProcess()} not available to remove from cache!");
                    //await ("Record not available").SendCacheFailedEmail();
                    return (false, Status.Failed);
                }
                //here we will update all the records again in cache
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Removing record {_cacheKey.MarkProcess()} from cache!");
                await CacheService.SetDataAsync(GenerateSpecificKey ? GenerateSpecificKeyForCache(key) : GenerateKeyForCache<T>(key, _clientId), modelRecords, _expirationTime);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Record {_cacheKey.MarkProcess()} removed from cache!");
                return (true, Status.Success);

            }
            catch (Exception e)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process End - {_cacheKey.MarkProcess()}  On removing record from cache, Exception occurred{e.Message}!");
                await e.SendExceptionMailAsync();
                return (false, e.Message);
            }

        }
        /// <summary>
        /// Generate cache key for get/set data inside cache
        /// </summary>
        /// <typeparam name="T">Object model, with this name the key get generated</typeparam>
        /// <param name="key">id for that record to generate key</param>
        /// <returns></returns>
        public string GenerateKeyForCache<T>(string key, string clientId)
        {
            return typeof(T).Name + $"|{key}|{clientId}";
        }
        /// <summary>
        /// Generate cache key for get/set data inside cache
        /// </summary>
        /// <typeparam name="T">Object model, with this name the key get generated</typeparam>
        /// <param name="key">id for that record to generate key</param>
        /// <returns></returns>
        private string GenerateSpecificKeyForCache(string key)
        {
            return $"{key}";
        }
    }
}
