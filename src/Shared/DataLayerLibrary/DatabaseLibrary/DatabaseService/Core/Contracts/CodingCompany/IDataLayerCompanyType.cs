using DBOperationsLayer.Data.Context;
using ModelTemplates.EntityModels.Company;

namespace DataBaseServices.Core.Contracts.CodingCompany;

public interface IDataLayerCompanyType
{
    Task<CompanyTypeModel> Add(CompanyTypeModel model);
    Task<List<CompanyTypeModel>> AddRange(List<CompanyTypeModel> modelRecords);
    Task AddMultiple(CompanyTypeModel[] models);
    Task<CompanyTypeModel?> Get(string Id);
    Task<CompanyTypeModel?> GetCompanyTypeAsPerEnum(string name);
    Task<List<CompanyTypeModel>> GetAll();
    Task Remove(string Id);
    Task<CompanyTypeModel> Update(CompanyTypeModel model);
    Task<ApplicationDbContext> GetDbContextAsyncAsync();
    Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
}
