using DBOperationsLayer.Data.Context;
using ModelTemplates.Master.Company;

namespace DataBaseServices.Core.Contracts.CodingCompany;

public interface IDataLayerCompanyMasterProfile
{
    Task<CompanyMasterProfileModel> Add(CompanyMasterProfileModel model);
    Task<CompanyMasterProfileModel?> Get(string Id);
    Task<CompanyMasterProfileModel?> GetCompanyAsPerCompanyTypeId(string companyTypeId);
    Task<List<CompanyMasterProfileModel>> GetAll();
    Task<IQueryable<CompanyMasterProfileModel>> GetAllAsQuariable();
    Task Remove(string Id);
    Task<CompanyMasterProfileModel> Update(CompanyMasterProfileModel model);
    Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
    Task<ApplicationDbContext> GetDbContextAsyncAsync();

}