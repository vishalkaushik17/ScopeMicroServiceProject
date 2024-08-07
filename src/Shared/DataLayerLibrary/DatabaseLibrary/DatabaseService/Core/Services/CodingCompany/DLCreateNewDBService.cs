using DataBaseServices.Core.Contracts.CodingCompany;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.EntityModels.UserAccount;

namespace DataBaseServices.Core.Services.CodingCompany
{
    public sealed class DLCreateNewDBService : IDataLayerNewDBService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITrace _trace;

        public DLCreateNewDBService(ApplicationDbContext context, ITrace trace)
        {
            _dbContext = context;
            this._trace = trace;
        }
        public async Task<ApplicationDbContext> GetDbContext()
        {
            return _dbContext;
        }
        public Task<ApplicationDbContext> SetDbContext(ApplicationDbContext dbContext)
        {
            throw new NotImplementedException();
        }
        public ApplicationDbContext GetNewConnection(IHttpContextAccessor httpContextAccessor)
        {
            return new ApplicationDbContext(options: _dbContext._Options, httpContextAccessor, _trace);
        }
        public ApplicationDbContext ConnectToDifferentDB(IHttpContextAccessor httpContextAccessor)
        {
            return new ApplicationDbContext(options: _dbContext._Options, httpContextAccessor, _trace);
        }

        public async Task<AccountConfirmationModel?> AccountConfirmation(ApplicationDbContext context, string hostid, string activationId)
        {
            return await context.AccountConfirmations.
            FirstOrDefaultAsync(m => m.Id == activationId && m.HostId == hostid
                                && m.RecordStatus == EnumRecordStatus.Active && !m.IsConfirmed);
        }
        public async Task<ApplicationUser?> FindUser(ApplicationDbContext context, string userId)
        {
            return await context.User.FirstOrDefaultAsync(m => m.Id == userId && m.RecordStatus == EnumRecordStatus.Active);
        }
        public async Task ApplyMigration()
        {
            await Task.Run(() => _dbContext.Database.Migrate());
        }
    }
}
