using DataBaseServices.Core.Contracts.UsersAndRoles;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.Core.Services.UsersAndRoles
{
    public sealed class DLRoleService : BaseGenericRepository<UserRoles>, IDataLayerRoleContract

    {
        public DLRoleService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
        {

        }
        /// <summary>
        /// Get all active roles
        /// </summary>
        /// <returns>list of active roles</returns>
        public async Task<List<UserRoles>> GetAll()
        {
            return await _dbContext.UserRolesMaster.Where(model => model.RecordStatus == GenericFunction.Enums.EnumRecordStatus.Active).ToListAsync();
        }
    }
}
