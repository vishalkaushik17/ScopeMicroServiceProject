using GenericFunction.ResultObject;
using ModelTemplates.DtoModels.Inventory;

namespace BSLayerSchool.BSInterfaces.CommonContracts;

/// <summary>
/// Business logic for Vendor
/// </summary>
public interface IBsVendorContract
{
    /// <summary>
    /// Get record by id
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return match record</returns>
    Task<ResponseDto<VendorDtoModel>> Get(string id, bool useCache = true);

    /// <summary>
    /// Get all records
    /// </summary>
    /// <param name="pageNo">Page NO</param>
    /// <param name="pageSize">No of records on Page.</param>
    /// <param name="useCache">true/false</param>
    /// <returns>Return all matching records</returns>
    Task<ResponseDto<List<VendorDtoModel>>> GetAll(int pageNo, int pageSize,
        bool useCache = true);

    /// <summary>
    /// Add a new record.
    /// </summary>
    /// <param name="dto">Dto Record</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return newly updated record.</returns>/returns>
    Task<ResponseDto<VendorDtoModel>> AddAsync(VendorDtoModel dto, bool useCache = true);

    /// <summary>
    /// Update existing record.
    /// </summary>
    /// <param name="id">record Id</param>
    /// <param name="dto">Dto model</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return updated record</returns>
    Task<ResponseDto<VendorDtoModel>> UpdateAsync(string id, VendorDtoModel dto,
        bool useCache = true);

    /// <summary>
    /// Delete record
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="useCache">true false</param>
    /// <returns>return deleted record.</returns>
    Task<ResponseDto<VendorDtoModel>> DeleteAsync(string id, bool useCache = true);
}