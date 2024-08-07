using ModelTemplates.Persistence.Models.School.Employee;

namespace DataBaseServices.Core.Contracts.CommonContracts;

/// <summary>
/// For Managing Department
/// </summary>
public interface IDataLayerDepartmentContract
{

    /// <summary>
    /// Return record
    /// </summary>
    /// <param name="id">record id</param>
    /// <returns></returns>
    Task<DepartmentModel?> GetAsync(string id);

    /// <summary>
    /// Get all records.
    /// </summary>
    /// <returns>Get list of records.</returns>
    Task<List<DepartmentModel>?> GetAllAsync();

    /// <summary>
    /// Add new record to database.
    /// </summary>
    /// <param name="modelRecord">model record</param>
    /// <returns>return newly inserted record with id.</returns>
    Task<DepartmentModel> AddAsync(DepartmentModel modelRecord);

    /// <summary>
    /// Update record in database.
    /// </summary>
    /// <param name="modelRecord">Model record</param>
    /// <returns>return updated record.</returns>
    Task<DepartmentModel> UpdateAsync(DepartmentModel modelRecord);

    /// <summary>
    /// Delete record from database.
    /// </summary>
    /// <returns>return true if record deleted.</returns>
    Task<DepartmentModel> DeleteAsync(DepartmentModel modelRecord);

    /// <summary>
    /// Check library record is used in another table like room
    /// </summary>
    /// <param name="id">Library id</param>
    /// <returns>Return true is record is in use.</returns>
    Task<bool> IsRecordInUseAsync(string id);
}
