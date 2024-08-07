using DataBaseServices.Core.Contracts.UsersAndRoles;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.Core.Services.UsersAndRoles
{

    public sealed class DLUserService : BaseGenericRepository<ApplicationUser>, IDataLayerUserContract
    {

        public DLUserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
            base(dbContext, httpContextAccessor, trace)
        {

            //No Implementations required. Everything is set in base class
        }
        public async Task<ApplicationUser?> Get(string id)
        {
            //await SetDbContext(new ApplicationDbContext(_dbContext._Options, _httpContextAccessor, _trace));
            return await _dbContext.User.Include(i => i.Company).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<ApplicationUser>?> GetUsersByClientId(string ClientId)
        {
            //await SetDbContext(new ApplicationDbContext(_dbContext._Options, _httpContextAccessor, _trace));
            return await _dbContext.User.Include(i => i.Company).Where(m => m.CompanyId == ClientId).ToListAsync();
        }
    }
}
