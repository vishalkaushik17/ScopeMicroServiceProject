using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Application;

namespace DataBaseServices.LayerRepository.Library;

public class UserRepository : BaseGenericRepository<ApplicationUser>, IUserContract
{
    public UserRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {

    }

    public async Task<ApplicationUser?> GetByName(string? name = "")
    {
        return await _dbContext.User.FirstOrDefaultAsync(model => model.UserName == name);



    }

    public async Task<ApplicationUser?> GetById(string? id)
    {
        return await _dbContext.User.FirstOrDefaultAsync(model => model.Id == id && model.RecordStatus == EnumRecordStatus.Active);



    }
}