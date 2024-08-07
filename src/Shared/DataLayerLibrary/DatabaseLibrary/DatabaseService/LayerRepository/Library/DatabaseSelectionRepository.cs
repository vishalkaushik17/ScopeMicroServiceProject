using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.DatabaseConfig;

namespace DataBaseServices.LayerRepository.Library;
public class DatabaseSelectionRepository : BaseGenericRepository<DatabaseConnection>, IDatabaseSelectionContract
{
    public DatabaseSelectionRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {


    }

    public async Task<DatabaseConnection?> GetCatelog(string? catalogName = "")
    {
        return await _dbContext.DatabaseConnections.FirstOrDefaultAsync(model => model.Catalog == catalogName);
    }

    public Task<DatabaseConnection?> GetById(string? id)
    {
        throw new NotImplementedException();
    }


    //public async Task<DatabaseConnection?> IDatabaseSelectionContract.GetCatelog(string? catalogName)
    //{
    //    await _dbSet.FirstOrDefaultAsync(model => model.Catalog == catalogName);
    //}
}