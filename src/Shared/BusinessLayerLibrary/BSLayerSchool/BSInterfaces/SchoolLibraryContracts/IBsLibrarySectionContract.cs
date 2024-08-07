using GenericFunction.ResultObject;
using ModelTemplates.DtoModels.SchoolLibrary;

namespace BSLayerSchool.BSInterfaces.SchoolLibraryContracts;

/// <summary>
/// Business logic for Library Section process
/// </summary>
public interface IBsLibrarySectionContract
{
    /// <summary>
    /// Get record by id
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="roomId">roomId</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return match record</returns>
    Task<ResponseDto<LibrarySectionDtoModel>> Get(string id, string roomId, bool useCache = true);

    /// <summary>
    /// Get all records
    /// </summary>
    /// <param name="roomId">roomId</param>
    /// <param name="pageNo">Page NO</param>
    /// <param name="pageSize">No of records on Page.</param>
    /// <param name="useCache">true/false</param>
    /// <returns>Return all matching records</returns>
    Task<ResponseDto<List<LibrarySectionDtoModel>>> GetAll(string roomId, int pageNo, int pageSize,
        bool useCache = true);

    /// <summary>
    /// Add a new record.
    /// </summary>
    /// <param name="dto">Dto Record</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return newly updated record.</returns>/returns>
    Task<ResponseDto<LibrarySectionDtoModel>> AddAsync(LibrarySectionDtoModel dto, bool useCache = true);

    /// <summary>
    /// Update existing record.
    /// </summary>
    /// <param name="id">record Id</param>
    /// <param name="dto">Dto model</param>
    /// <param name="useCache">true/false</param>
    /// <returns>return updated record</returns>
    Task<ResponseDto<LibrarySectionDtoModel>> UpdateAsync(string id, LibrarySectionDtoModel dto,
        bool useCache = true);

    /// <summary>
    /// Delete record
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="roomId">roomId</param>
    /// <param name="useCache">true false</param>
    /// <returns>return deleted record.</returns>
    Task<ResponseDto<LibrarySectionDtoModel>> DeleteAsync(string id, string roomId, bool useCache = true);
}