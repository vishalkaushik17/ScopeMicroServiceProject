using DataBaseServices.Core.Contracts.CommonContracts;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using MCCommonLayer.Interface;
using ModelTemplates.Persistence.Models.School.CommonModels;

namespace MCCommonLayer.Repository
{
    /// <summary>
    /// Memory cache Language service
    /// </summary>
    public class MCLanguageService : IMCLanguageContract
    {
        private readonly IDataLayerLanguageContract _dlService;
        private ICacheContract _cacheData;
        private DateTimeOffset _expirationTime;
        private ApplicationSettings _applicationSettings;
        public MCLanguageService(IDataLayerLanguageContract dlService, ICacheContract _cache, ITrace trace)
        {
            _dlService = dlService;
            _cacheData = _cache;
            _applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
            _expirationTime = _applicationSettings.ModuleCacheSettings.CommonModule.GetKeyLifeForCacheStorage();
        }

        /// <summary>
        /// Local private method to update cache when new record added or existing record get updated. It doesnt work on Database layer
        /// </summary>
        /// <param name="modelRecord">record object</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="isNew">Check for New record entry or its already exists.  True : will add a new record in cache. False: will update the existing one.</param>
        /// <returns>void</returns>
        private async Task UpdateMemoryCacheOnCRUDAsync(LanguageModel modelRecord, string clientId, bool isNew)
        {
            (bool status, string message) operationStatus = await _cacheData.UpdateCacheAsync<LanguageModel>("Index", modelRecord.Id, modelRecord, _expirationTime, isNew: isNew);
            if (operationStatus.status == false)
            {
                await GetAllCacheAsync(1, clientId, true);
            }
        }
        /// <summary>
        /// Adding object to cache server.
        /// </summary>
        /// <param name="modelRecord">record object</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Task Model record</returns>
        public async Task<LanguageModel> AddCacheAsync(LanguageModel modelRecord, string clientId, bool useCache)
        {
            modelRecord = await _dlService.AddAsync(modelRecord);

            await UpdateMemoryCacheOnCRUDAsync(modelRecord, clientId, true);

            return modelRecord;

        }



        /// <summary>
        /// Deleting cache entry for specified Id.
        /// </summary>
        /// <param name="id">Id for records which is going to be deleted.</param>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>If record deleted successfully, it will return true.</returns>
        public async Task<EnumRecordStatus> DeleteCacheAsync(string id, LanguageModel modelRecord, int _modificationInDays, bool useCache)
        {
            var record = await _dlService.DeleteAsync(modelRecord);
            if (useCache && record.RecordStatus == EnumRecordStatus.Deleted)
            {
                (bool status, string message) recordStatus = await _cacheData.RemoveFromCacheAsync<LanguageModel>("Index", id, _modificationInDays, _expirationTime);

                if (!recordStatus.status)
                {
                    return EnumRecordStatus.RecordNotFound;
                }


            }
            return record.RecordStatus;

        }




        /// <summary>
        /// Get record from cache server.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Return record as per specific Id.</returns>
        public async Task<LanguageModel?> GetCacheAsync(string id, int _modificationInDays, string clientId, bool useCache)
        {

            if (!useCache)
            {
                return await _dlService.GetAsync(id);
            }


            List<LanguageModel>? models = await GetAllCacheAsync(_modificationInDays, clientId, useCache);
            LanguageModel? model = null;
            if (models?.Count > 0)
            {
                return models.FirstOrDefault(m => m.Id == id);
            }
            else
            {
                models = await _dlService.GetAllAsync();
                if (models?.Count > 0)
                {
                    var temp = await _cacheData.AddCacheAsync<List<LanguageModel>>(_cacheData.GenerateKeyForCache<LanguageModel>("Index", clientId), models, _expirationTime);
                    if (temp.status == true)
                    {
                        return models.FirstOrDefault(m => m.Id == id);
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Return records as per clientId</returns>
        public async Task<List<LanguageModel>?> GetAllCacheAsync(int _modificationInDays, string clientId, bool useCache)
        {
            List<LanguageModel>? models = null;
            if (useCache)
                models = await _cacheData.ReadFromCacheAsync<LanguageModel>("Index", _modificationInDays);

            if (models?.Count > 0)
            {
                return models;
            }
            else
            {
                models = await _dlService.GetAllAsync();
                if (models?.Count > 0)
                {
                    if (useCache)
                    {
                        var temp = await _cacheData.AddCacheAsync<ICollection<LanguageModel>>(_cacheData.GenerateKeyForCache<LanguageModel>("Index", clientId), models, _expirationTime);
                        if (temp.status == true)
                        {
                            return models;
                        }
                    }
                    else
                    {
                        return models;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checking for record reference in another table.
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <returns>If reference found, it returns true.</returns>
        public async Task<bool> IsRecordInUseAsync(string id)
        {
            return await _dlService.IsRecordInUseAsync(id);

        }

        /// <summary>
        /// Updating record in cache server.
        /// </summary>
        /// <param name="modelRecord">Record which is going to be updated on cache server.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Returns newly updated record.</returns>
        public async Task<LanguageModel> UpdateCacheAsync(LanguageModel modelRecord, string clientId, bool UseCache)
        {
            await _dlService.UpdateAsync(modelRecord);

            if (UseCache)
                await UpdateMemoryCacheOnCRUDAsync(modelRecord, clientId, false);

            return modelRecord;
        }
    }
}
