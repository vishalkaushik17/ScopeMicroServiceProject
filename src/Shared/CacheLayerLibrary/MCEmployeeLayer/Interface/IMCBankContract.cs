using GenericFunction.Enums;
using ModelTemplates.Persistence.Models.School.Employee;

namespace MCEmployeeLayer.Interface
{
    /// <summary>
    /// Memory cache Bank service
    /// </summary>

    public interface IMCBankContract
    {


        /// <summary>
        /// Get record from cache server.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Return record as per specific Id.</returns>
        Task<BankModel?> GetCacheAsync(string id, int _modificationInDays, string clientId, bool useCache);

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Return records as per clientId</returns>
        Task<List<BankModel>?> GetAllCacheAsync(int _modificationInDays, string clientId, bool useCache);

        /// <summary>
        /// Adding object to cache server.
        /// </summary>
        /// <param name="modelRecord">record object</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Task Model record</returns>
        Task<BankModel> AddCacheAsync(BankModel modelRecord, string clientId, bool useCache);

        /// <summary>
        /// Updating record in cache server.
        /// </summary>
        /// <param name="modelRecord">Record which is going to be updated on cache server.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>Returns newly updated record.</returns>

        Task<BankModel> UpdateCacheAsync(BankModel modelRecord, string clientId, bool UseCache);

        /// <summary>
        /// Deleting cache entry for specified Id.
        /// </summary>
        /// <param name="id">Id for records which is going to be deleted.</param>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>If record deleted successfully, it will return true.</returns>
        Task<EnumRecordStatus> DeleteCacheAsync(string id, BankModel modelRecord, int _modificationInDays, bool useCache);


        /// <summary>
        /// Checking for record reference in another table.
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <returns>If reference found, it returns true.</returns>
        Task<bool> IsRecordInUseAsync(string id);
    }
}
