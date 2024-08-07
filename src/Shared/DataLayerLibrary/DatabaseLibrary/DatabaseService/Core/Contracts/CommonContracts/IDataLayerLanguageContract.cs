using ModelTemplates.Persistence.Models.School.CommonModels;

namespace DataBaseServices.Core.Contracts.CommonContracts;

/// <summary>
/// For Managing Language
/// </summary>
public interface IDataLayerLanguageContract
{

    /// <summary>
    /// Return record
    /// </summary>
    /// <param name="id">record id</param>
    /// <returns></returns>
    Task<LanguageModel?> GetAsync(string id);

    /// <summary>
    /// Get all records.
    /// </summary>
    /// <returns>Get list of records.</returns>
    Task<List<LanguageModel>?> GetAllAsync();

    /// <summary>
    /// Add new record to database.
    /// </summary>
    /// <param name="modelRecord">model record</param>
    /// <returns>return newly inserted record with id.</returns>
    Task<LanguageModel> AddAsync(LanguageModel modelRecord);

    /// <summary>
    /// Update record in database.
    /// </summary>
    /// <param name="modelRecord">Model record</param>
    /// <returns>return updated record.</returns>
    Task<LanguageModel> UpdateAsync(LanguageModel modelRecord);

    /// <summary>
    /// Delete record from database.
    /// </summary>
    /// <returns>return true if record deleted.</returns>
    Task<LanguageModel> DeleteAsync(LanguageModel modelRecord);

    /// <summary>
    /// Check library record is used in another table like room
    /// </summary>
    /// <param name="id">Library id</param>
    /// <returns>Return true is record is in use.</returns>
    Task<bool> IsRecordInUseAsync(string id);
}
