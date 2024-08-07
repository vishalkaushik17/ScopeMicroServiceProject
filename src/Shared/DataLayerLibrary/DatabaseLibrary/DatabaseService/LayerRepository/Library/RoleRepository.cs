using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.LayerRepository.Library;

public class RoleRepository : BaseGenericRepository<UserRoles>, IRolesContract
{
    public RoleRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {
    }

    public async Task<UserRoles?> GetByName(string? name = "")
    {
        return await _dbContext.UserRolesMaster.FirstOrDefaultAsync(model => model.Name == name);
    }

    public async Task<UserRoles?> GetById(string? id)
    {
        return await _dbContext.UserRolesMaster.FirstOrDefaultAsync(model => model.Id == id && model.RecordStatus == EnumRecordStatus.Active);
    }

    public IQueryable<UserRoles> All()
    {
        return _dbContext.UserRolesMaster.AsQueryable();
    }
}