using GenericFunction.ResultObject;
using ModelTemplates.DtoModels.Company;

namespace BSCodingCompany.BSInterfaces.DemoRequest;

/// <summary>
/// Business logic for Demo Request Layer process
/// </summary>
public interface IBsDemoRequestContract
{
    /// <summary>
    /// Get Demo Request Entity Model by registered email demoId.
    /// </summary>
    /// <param name="emailId">Registered emailId</param>
    /// <returns>return entity model</returns>
    Task<ResponseDto<DemoRequestDtoModel>> GetByEmailId(string emailId);


    /// <summary>
    /// Get Demo Request Entity Model by registered Contact No.
    /// </summary>
    /// <param name="contactNo">Registered Contact No.</param>
    /// <returns>return entity model</returns>
    Task<ResponseDto<DemoRequestDtoModel>> GetByContactNo(string contactNo);

    /// <summary>
    /// Get Demo Request Entity Model by registered Contact No.
    /// </summary>
    /// <param name="website">Registered Website/Domain.</param>
    /// <returns>return entity model</returns>
    Task<ResponseDto<DemoRequestDtoModel>> GetByWebsite(string website);


    /// <summary>
    /// Get Demo Request Entity Model by registered reference no.
    /// </summary>
    /// <param name="referenceCode">Reference no on which demo is requested.</param>
    /// <returns>return entity model</returns>
    Task<ResponseDto<DemoRequestDtoModel>> GetByReferenceCode(string referenceCode);


    /// <summary>
    /// Get all Demo Request Entity Models by registered reference no.
    /// </summary>
    /// <param name="referenceCode">Reference no on which demo is requested.</param>
    /// <returns>return entity model</returns>
    Task<ResponseDto<List<DemoRequestDtoModel>>> GetAllByReferenceCode(string referenceCode);

    /// <summary>
    /// Get all demo request.
    /// </summary>
    /// <param name="isActive">bool true/false for active or pending for activation.</param>
    /// <returns></returns>
    Task<ResponseDto<List<DemoRequestDtoModel>>> GetAll(bool isActive);

    Task<ResponseDto<DemoRequestDtoModel>> GetDemoRequestById(string id);

    Task<ResponseDto<DemoRequestDtoModel>> GetByAny(string? id = "", string? refCode = "", string? website = "", string? contactNo = "", string? email = "");
    /// <summary>
    /// Check Demo Request Model for already registered in our record.
    /// </summary>
    /// <param name="dto">Demo Request Dto Model</param>
    /// <returns>returns Response Dto object with Demo Request Dto Template</returns>
    Task<ResponseDto<DemoRequestDtoModel>> GenerateDemoRequest(DemoRequestDtoModel dto);

    /// <summary>
    /// It will activate demo request.
    /// </summary>
    /// <param name="dto">Demo Request Dto object</param>
    /// <returns>Returns Response object</returns>
    Task<ResponseDto<CompanyMasterDtoModel>> ActivateDemo(string demoId, string hostId, string? customSuffixDomain, string? refCode);



}
