using DBOperationsLayer.Data.Context;
using ModelTemplates.EntityModels.AppConfig;

namespace DataBaseServices.Core.Contracts.CodingCompany;

public interface IDataLayerCompanyMasterVsDbHost
{
    Task<AppDBHostVsCompanyMaster> Add(AppDBHostVsCompanyMaster model);
    Task<AppDBHostVsCompanyMaster?> Get(string Id);
    Task<List<AppDBHostVsCompanyMaster>> GetAll();
    Task<List<AppDBHostVsCompanyMaster>> GetAllWithProfileAndHost();
    Task<AppDBHostVsCompanyMaster?> GetAllWithProfileAndHost(string suffixDomain);
    Task<IQueryable<AppDBHostVsCompanyMaster>> GetAllAsQuariable();
    Task Remove(string Id);
    Task<AppDBHostVsCompanyMaster> Update(AppDBHostVsCompanyMaster model);
    Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
    Task<ApplicationDbContext> GetDbContextAsyncAsync();
}
