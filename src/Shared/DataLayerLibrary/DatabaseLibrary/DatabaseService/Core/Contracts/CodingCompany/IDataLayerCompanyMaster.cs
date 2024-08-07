using DBOperationsLayer.Data.Context;
using ModelTemplates.Master.Company;

namespace DataBaseServices.Core.Contracts.CodingCompany;

public interface IDataLayerCompanyMaster
{
    Task<CompanyMasterModel> AddAsync(CompanyMasterModel model);
    Task<CompanyMasterModel?> GetAsync(string Id);
    Task<CompanyMasterModel?> GetSchoolByDemoRequestIdAsync(string DemoRequestId);
    Task<CompanyMasterModel?> GetCompanyByReferenceCodeAsync(string referenceCode);
    Task<CompanyMasterModel?> GetCompanyAsPerCompanyTypeIdAsync(string companyTypeId);
    Task<bool> GetSchoolInDemoModeAsync(string Id);
    Task<List<CompanyMasterModel>> GetAllAsync();
    Task<IQueryable<CompanyMasterModel>> GetAllAsQuariableAsync();
    Task RemoveAsync(string Id);
    Task<CompanyMasterModel> UpdateAsync(CompanyMasterModel model);
    Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
    Task<ApplicationDbContext> GetDbContextAsyncAsync();

}
