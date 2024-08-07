using DBOperationsLayer.Data.Context;
using ModelTemplates.Master.Company;

namespace DataBaseServices.Core.Contracts.CodingCompany;

/// <summary>
/// Demo Request for School
/// </summary>
public interface IDataLayerDemoRequestContract //: IGenericContract<DemoRequestModel, DemoRequestModel>
{
    /// <summary>
    /// Get Demo Request Entity Model by registered email demoId.
    /// </summary>
    /// <param name="emailId">Registered emailId</param>
    /// <returns>return entity model</returns>
    Task<DemoRequestModel?> GetByEmailId(string emailId);

    Task<DemoRequestModel?> GetNewDemoRequest(string demoId);
    /// <summary>
    /// Get Demo Request Entity Model by registered Contact No.
    /// </summary>
    /// <param name="contactNo">Registered Contact No.</param>
    /// <returns>return entity model</returns>
    Task<DemoRequestModel?> GetByContactNo(string contactNo);

    /// <summary>
    /// Get Demo Request Entity Model by registered Contact No.
    /// </summary>
    /// <param name="website">Registered Website/Domain.</param>
    /// <returns>return entity model</returns>
    Task<DemoRequestModel> GetByWebsite(string website);


    /// <summary>
    /// Get Demo Request Entity Model by registered reference no.
    /// </summary>
    /// <param name="referenceCode">Reference no on which demo is requested.</param>
    /// <returns>return entity model</returns>
    Task<DemoRequestModel?> GetByReferenceCode(string referenceCode);

    /// <summary>
    /// Check Demo Request Entity Model by registered reference no.
    /// </summary>
    /// <param name="referenceCode">Reference no on which demo is requested.</param>
    /// <returns>return bool</returns>
    Task<bool> CheckByReferenceCode(string referenceCode);

    /// <summary>
    /// Get all Demo Request Entity Models by registered reference no.
    /// </summary>
    /// <param name="referenceCode">Reference no on which demo is requested.</param>
    /// <returns>return entity model</returns>
    Task<List<DemoRequestModel>> GetAllByReferenceCode(string referenceCode);

    /// <summary>
    /// Get demo requests.
    /// </summary>
    /// <param name="isActive">true/false</param>
    /// <returns>return demo requests.</returns>
    Task<List<DemoRequestModel>> GetAll(bool isActive);

    Task<DemoRequestModel> GetDemoRequestById(string id);

    Task<DemoRequestModel> GetByAny(string? id = "", string? refCode = "", string? website = "", string? contactNo = "", string? email = "");

    /// <summary>
    /// Check Demo Request Model for already registered in our record.
    /// </summary>
    /// <param name="dto">Demo Request Dto Model</param>
    /// <returns>returns Response Dto object with Demo Request Dto Template</returns>
    Task<DemoRequestModel> GenerateDemoRequest(DemoRequestModel dto);

    /// <summary>
    /// It will activate demo request.
    /// </summary>
    /// <param name="dto">Demo Request Dto object</param>
    /// <returns>Returns Response object</returns>
    //Task<CompanyMasterModel?> ActivateDemo(string demoId, string? customSuffixDomain, string hostId);
    Task<DemoRequestModel?> ActivateDemo(string demoId, string? customSuffixDomain, string hostId);
    //Task<ResponseDto<CompanyMasterDtoModel>> ActivateDemo(string demoId, string? customSuffixDomain, string userId, string hostId, string origin);

    Task<DemoRequestModel?> IsDemoAlreadyRequested(string website, string contactNo, string email);

    Task<DemoRequestModel> Update(DemoRequestModel model);
    Task<DemoRequestModel> Add(DemoRequestModel model);
    Task<ApplicationDbContext> GetDbContextAsyncAsync();
    Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
}

