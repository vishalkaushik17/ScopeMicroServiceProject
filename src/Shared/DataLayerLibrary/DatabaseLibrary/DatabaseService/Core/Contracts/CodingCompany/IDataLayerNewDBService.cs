using DBOperationsLayer.Data.Context;
using Microsoft.AspNetCore.Http;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.UserAccount;

namespace DataBaseServices.Core.Contracts.CodingCompany
{
    public interface IDataLayerNewDBService
    {
        ApplicationDbContext GetNewConnection(IHttpContextAccessor httpContextAccessor);
        ApplicationDbContext ConnectToDifferentDB(IHttpContextAccessor httpContextAccessor);
        Task<AccountConfirmationModel?> AccountConfirmation(ApplicationDbContext context, string hostid, string activationId);
        Task<ApplicationUser?> FindUser(ApplicationDbContext context, string userId);
        Task<ApplicationDbContext> GetDbContext();
        Task<ApplicationDbContext> SetDbContext(ApplicationDbContext dbContext);
        Task ApplyMigration();
    }
}
