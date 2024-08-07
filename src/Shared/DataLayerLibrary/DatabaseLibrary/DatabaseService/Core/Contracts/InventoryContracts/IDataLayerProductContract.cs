using ModelTemplates.Persistence.Models.School.Inventory;

namespace DataBaseServices.Core.Contracts.CommonContracts;

/// <summary>
/// For Managing Product
/// </summary>
public interface IDataLayerProductContract
{

    /// <summary>
    /// Return record
    /// </summary>
    /// <param name="id">record id</param>
    /// <returns></returns>
    Task<ProductModel?> GetAsync(string id);

    /// <summary>
    /// Get all records.
    /// </summary>
    /// <returns>Get list of records.</returns>
    Task<List<ProductModel>?> GetAllAsync();

    /// <summary>
    /// Add new record to database.
    /// </summary>
    /// <param name="modelRecord">model record</param>
    /// <returns>return newly inserted record with id.</returns>
    Task<ProductModel> AddAsync(ProductModel modelRecord);

    /// <summary>
    /// Update record in database.
    /// </summary>
    /// <param name="modelRecord">Model record</param>
    /// <returns>return updated record.</returns>
    Task<ProductModel> UpdateAsync(ProductModel modelRecord);

    /// <summary>
    /// Delete record from database.
    /// </summary>
    /// <returns>return true if record deleted.</returns>
    Task<ProductModel> DeleteAsync(ProductModel modelRecord);

    /// <summary>
    /// Check library record is used in another table like room
    /// </summary>
    /// <param name="id">Library id</param>
    /// <returns>Return true is record is in use.</returns>
    Task<bool> IsRecordInUseAsync(string id);
}
