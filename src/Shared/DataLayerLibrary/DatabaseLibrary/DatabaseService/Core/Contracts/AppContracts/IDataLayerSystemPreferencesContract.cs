using ModelTemplates.Persistence.Models.AppLevel;

namespace DataBaseServices.Core.Contracts.AppContracts
{
    public interface IDataLayerSystemPreferencesContract
    {
        Task<List<SystemPreferencesModel>> GetAllAsync();
        Task<SystemPreferencesModel?> GetAsync(string id);
        Task<SystemPreferencesModel> AddAsync(SystemPreferencesModel model);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<SystemPreferencesModel> UpdateAsync(SystemPreferencesModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check library record is used in another table like room
        /// </summary>
        /// <param name="id">Library id</param>
        /// <returns>Return true is record is in use.</returns>
        Task<bool> IsRecordInUseAsync(string id);
        /// <summary>
        /// Find record by name.
        /// </summary>
        /// <param name="name">Name to find in table.</param>
        /// <param name="id">Id for checking record with found record by name.</param>
        /// <returns>Return true if record found.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? id);

    }
}
