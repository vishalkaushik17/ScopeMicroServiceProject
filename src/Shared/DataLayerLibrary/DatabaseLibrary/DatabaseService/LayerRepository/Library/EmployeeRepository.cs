//using AutoMapper;
//using DBOperationsLayer.Data.Context;
//using GenericFunction.Helpers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using ModelTemplates.Core.Model;

//namespace DataBaseServices.LayerRepository.Library;

//public class EmployeeRepository //: BaseGenericRepository<EmployeeModel, EmployeeDtoAbstractModel>, IEmployeeContract
//{

//    private readonly DbSet<EmployeeModel> _dbSet;



//    public EmployeeRepository(ApplicationDbContext dbContext, IMapper mapper,
//        IOptions<MailConfiguration> appSettings, IHttpContextAccessor httpContextAccessor) :
//        base(dbContext, mapper, httpContextAccessor)
//    {
//        //_dbContext = dbContext;
//        //_mapper = mapper;


//        _dbSet = dbContext.Set<EmployeeModel>();

//    }

//    public async Task GetByName(string? name = "")
//    {
//        await _dbSet.FirstOrDefaultAsync(model => model.Name == name);

//    }

//    //public async Task<string?> Save(EmployeeEntityTemplate model, ExecuteWith exeWith)
//    //{
//    //    //model.Id = Guid.NewGuid().ToString("D");
//    //    var arrParameters = new[]
//    //    {
//    //        new SqlParameter() {ParameterName = SpGetAirline.Id, SqlDbType = SqlDbType.NVarChar, Value= model.Id = Guid.NewGuid().ToString("D")},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.AirportName, SqlDbType = SqlDbType.NVarChar, Value= model.AirportName},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.Country, SqlDbType = SqlDbType.NVarChar, Value= model.Country},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.City, SqlDbType = SqlDbType.NVarChar, Value= model.City},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.AirportCode, SqlDbType = SqlDbType.NVarChar, Value= model.AirportCode},
//    //        //new SqlParameter() {ParameterName = SpGetAirline.OutId, SqlDbType = SqlDbType.NVarChar, Value= model.AirportCode,Direction=System.Result.ParameterDirection.Output, Size=450}
//    //    };

//    //    if (exeWith == ExecuteWith.Ado)
//    //    {
//    //        DbExecution dbExecute = new();
//    //        (int records, SqlCommand sqlCmd) = dbExecute.ADOExecuteDMLSP(SpSaveAirline.ToName(), arrParameters);

//    //        if (records > 0)
//    //        {
//    //            return sqlCmd?.Parameters[SpGetAirline.OutId].Value.ToString();
//    //        }
//    //    }
//    //    else
//    //    {
//    //        DbExecution dbExecute = new(DbContext);

//    //        (int records, Dictionary<string, SqlParameter> outParams) = dbExecute.EntityExecuteDMLSP(SpSaveAirline.ToName(), arrParameters);

//    //        if (records > 0)
//    //        {
//    //            var myValue = outParams.FirstOrDefault(x => x.Key == SpGetAirline.OutId);
//    //            return (string)myValue.Value.Value;
//    //        }
//    //    }

//    //    return null;
//    //}
//}