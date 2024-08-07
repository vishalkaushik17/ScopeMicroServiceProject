using GenericFunction.ResultObject;
using ModelTemplates.DtoModels.Employee;

namespace BSLayerSchool.BSInterfaces.EmployeeContracts;

/// <summary>
/// Business logic for DesignationMasterDtoModel
/// </summary>
public interface IBsDesignationMasterContract
{
    /// <summary>
    /// Get record by id
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return match record</returns>
    Task<ResponseDto<DesignationDtoModel>> Get(string id, bool useCache = true);

    /// <summary>
    /// Get all records
    /// </summary>
    /// <param name="pageNo">Page NO</param>
    /// <param name="pageSize">No of records on Page.</param>
    /// <param name="useCache">true/false</param>
    /// <returns>Return all matching records</returns>
    Task<ResponseDto<List<DesignationDtoModel>>> GetAll(int pageNo, int pageSize,
        bool useCache = true);

    /// <summary>
    /// Add a new record.
    /// </summary>
    /// <param name="dto">Dto Record</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return newly updated record.</returns>/returns>
    Task<ResponseDto<DesignationDtoModel>> AddAsync(DesignationDtoModel dto, bool useCache = true);

    /// <summary>
    /// Update existing record.
    /// </summary>
    /// <param name="id">record Id</param>
    /// <param name="dto">Dto model</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return updated record</returns>
    Task<ResponseDto<DesignationDtoModel>> UpdateAsync(string id, DesignationDtoModel dto,
        bool useCache = true);

    /// <summary>
    /// Delete record
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="useCache">true false</param>
    /// <returns>return deleted record.</returns>
    Task<ResponseDto<DesignationDtoModel>> DeleteAsync(string id, bool useCache = true);
}