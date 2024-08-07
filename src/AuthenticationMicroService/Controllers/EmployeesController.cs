//using AutoMapper;
//using GenericFunction.Constants.Authorization;
//using GenericFunction.ResultObject;

//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using ModelTemplates.Core.Model;
//using ModelTemplates.DtoModels.Employee;
//using ModelTemplates.EntityModels.Application;
//using SharedLibrary.Services;
//using UnitOfWork;
//using UnitOfWork.DI;
//using static GenericFunction.CommonMessages;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace AuthenticateService.Controllers;

//[ApiController]
//[CustomAuthorize]
//[ApiVersion("1.0")]
//[Route("api/v{version:apiVersion}/[controller]")]
////[ModelStateValidationActionFilter]
////[ServiceFilter(typeof(ValidationFilterAttribute))]

////public class EmployeesController : ApiBaseController, ICommonApiMethods<ResponseDto<EmployeeDtoAbstractModel>,EmployeeDtoAbstractModel>

//public class EmployeesController : ApiBaseController<EmployeeDtoAbstractModel>//, ICommonApiMethods<EmployeeDtoAbstractModel>
//{
//    private readonly IConfiguration _configuration;

//    #region Constructor


//    public EmployeesController(IUnitOfWorkService unitOfWorkService,
//        IMapper mapper, ITrace trace,
//        UserManager<ApplicationUser> userManager,
//        IHttpContextAccessor httpContextAccessor, ITokenService tService, IConfiguration configuration) :
//        base(unitOfWorkService, mapper, trace, userManager, httpContextAccessor, tService)
//    {
//        _configuration = configuration;
//    }

//    #endregion




//    #region Api Methods for Controller

//    [HttpGet]
//    [AllowAnonymous]
//    [Route(nameof(Ping))]
//    public string Ping()
//    {
//        return "Success!";
//    }


//    [HttpGet]
//    [MapToApiVersion("1.0")]
//    [Route("Emp")]
//    //[AllowAnonymous]
//    [CustomAuthorize(RoleName.Administrator, RoleName.AsstStore)]
//    public string Emp()
//    {
//        //int i = 0;
//        //var b = 10 / i;
//        var refreshToken = Request.Cookies["refreshToken"];
//        return "emp success";
//    }

//    //// GET: api/<EmployeesController>
//    //[HttpGet]
//    //[MapToApiVersion("1.0")]
//    //[Route("GetEmployees")]
//    //[AllowAnonymous]
//    ////[Authorize(RoleName.Master, RoleName.Administrator)]
//    //public override async Task<ResponseDto<List<EmployeeDtoAbstractModel>>> GetAll()
//    //{
//    //    //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
//    //    //var accessToken = await _HttpContextAccessor.HttpContext.GetTokenAsync("access_token");


//    //    //if (_bearer_token == null)
//    //    //{
//    //    //    return null;
//    //    //}
//    //    //if (!_tService.ValidateToken(_configuration[IssuerSigningKey].ToString(), _configuration[ValidIssuer].ToString(), _configuration[ValidAudience], accessToken))
//    //    //{
//    //    //    return null;
//    //    //}

//    //    //fetch all the records
//    //    //throw new Exception("test exception");
//    //    var records = await UnitOfWorkService.EmployeeContractContract.GetAllAsync().ToListAsync();
//    //    //var records = await UnitOfWorkService.EmployeeContractContract.GetAllAsync(m => m.UserId == UserId).ToListAsync();
//    //    var dtoRecords = Mapper.Map<List<EmployeeModel>, List<EmployeeDtoAbstractModel>>(records);
//    //    return
//    //        new ResponseDto<List<EmployeeDtoAbstractModel>>
//    //        {
//    //            Result = dtoRecords,
//    //            ClientId = ClientId,
//    //            RecordCount = dtoRecords.Count,
//    //            Status = Status.Success,
//    //            Log = Trace.GetTraceLogs(),
//    //            TimeConsumption = Trace.TimeConsumption(),
//    //            Message = OperationSuccessful,
//    //            StatusCode = StatusCodes.Status200OK,
//    //        };
//    //}

//    // GET api/<EmployeesController>/5
//    [HttpGet]
//    [MapToApiVersion("1.0")]
//    //[Route("GetEmployee/{id:alpha}")]
//    [Route("GetEmployee/{id}")]
//    public async Task<ResponseDto<EmployeeDtoAbstractModel>> Get(string id)
//    {
//        if (string.IsNullOrWhiteSpace(id))
//            return
//                new ResponseDto<EmployeeDtoAbstractModel>
//                {
//                    Result = null,
//                    ClientId = ClientId,
//                    RecordCount = 0,
//                    Status = Status.Failed,
//                    Log = Trace.GetTraceLogs(),
//                    TimeConsumption = Trace.TimeConsumption(),
//                    Message = OperationFailed,
//                    MessageType = MessageType.Warning,
//                    StatusCode = StatusCodes.Status404NotFound
//                };
//        var record = await UnitOfWorkService.EmployeeContractContract.GetAsync(model => model.Id == id && model.UserId == UserId);
//        if (record == null)
//            return
//                new ResponseDto<EmployeeDtoAbstractModel>
//                {
//                    Result = null,
//                    ClientId = ClientId,
//                    RecordCount = 0,
//                    Status = Status.Failed,
//                    Log = Trace.GetTraceLogs(),
//                    TimeConsumption = Trace.TimeConsumption(),
//                    Message = OperationFailed,
//                    StatusCode = StatusCodes.Status404NotFound
//                };
//        var dtoRecord = Mapper.Map<EmployeeDtoAbstractModel>(record);

//        return
//            new ResponseDto<EmployeeDtoAbstractModel>
//            {
//                Result = dtoRecord,
//                RecordCount = 1,
//                Status = Status.Success,
//                Log = Trace.GetTraceLogs(),
//                TimeConsumption = Trace.TimeConsumption(),
//                Message = OperationSuccessful,
//                StatusCode = StatusCodes.Status200OK
//            };
//    }

//    // POST api/<EmployeesController>
//    [HttpPost]
//    [MapToApiVersion("1.0")]
//    [Route("SaveEmployee")]
//    //[Authorize(Roles = "Admin")]
//    [CustomAuthorize(RoleName.Master, RoleName.Administrator)]

//    public async Task<ResponseDto<EmployeeDtoAbstractModel>> Add([FromBody] EmployeeDtoAbstractModel dtoAbstractModel)
//    {
//        var record = Mapper.Map<EmployeeModel>(dtoAbstractModel);
//        if (UserId != null) record.Save(UserId);

//        await UnitOfWorkService.EmployeeContractContract.AddAsync(record);
//        var dtoRecord = Mapper.Map<EmployeeDtoAbstractModel>(record);
//        await UnitOfWorkService.CommitAsync();

//        return
//            new ResponseDto<EmployeeDtoAbstractModel>
//            {
//                Id = dtoRecord.Id,
//                ClientId = ClientId,
//                Result = dtoRecord,
//                RecordCount = 1,
//                Status = Status.Success,
//                Log = Trace.GetTraceLogs(),
//                TimeConsumption = Trace.TimeConsumption(),
//                Message = OperationSuccessful,
//                StatusCode = StatusCodes.Status201Created
//            };
//    }

//    // PUT api/<EmployeesController>/5
//    [HttpPost]
//    [MapToApiVersion("1.0")]
//    [Route("UpdateEmployee/{id}")]
//    [CustomAuthorize(RoleName.Master, RoleName.Administrator)]

//    public async Task<ResponseDto<EmployeeDtoAbstractModel>> Update([FromRoute] string id, [FromBody] EmployeeDtoAbstractModel dtoAbstractModel)
//    {
//        var recordDb = UnitOfWorkService.EmployeeContractContract.GetAsync(c => c.Id == id && c.UserId == UserId).Result;
//        if (recordDb == null)
//            return
//                new ResponseDto<EmployeeDtoAbstractModel>
//                {
//                    Result = null,
//                    RecordCount = 0,
//                    ClientId = ClientId,
//                    Status = Status.Failed,
//                    Log = Trace.GetTraceLogs(),
//                    TimeConsumption = Trace.TimeConsumption(),
//                    Message = OperationFailed,
//                    StatusCode = StatusCodes.Status404NotFound
//                };

//        var recordMapped = Mapper.Map(dtoAbstractModel, recordDb);

//        if (UserId != null)
//            recordMapped?.Update(UserId);

//        await UnitOfWorkService.CommitAsync();


//        return
//            new ResponseDto<EmployeeDtoAbstractModel>
//            {
//                Id = id,
//                ClientId = ClientId,
//                Result = dtoAbstractModel,
//                RecordCount = 1,
//                Status = Status.Success,
//                Log = Trace.GetTraceLogs(),
//                TimeConsumption = Trace.TimeConsumption(),
//                Message = OperationSuccessful,
//                StatusCode = StatusCodes.Status200OK
//            };

//        //return Ok(recordMapped);
//    }

//    // DELETE api/<EmployeesController>/5
//    [HttpPost]
//    [MapToApiVersion("1.0")]
//    [Route("DeleteEmployee/{id}")]
//    [CustomAuthorize(RoleName.Accountants)]
//    public async Task<ResponseDto<EmployeeDtoAbstractModel>> Delete(string id)
//    {

//        var recordDb = UnitOfWorkService.EmployeeContractContract.GetAsync(c => c.Id == id && c.UserId == UserId).Result;
//        if (recordDb == null)
//        {
//            return
//                new ResponseDto<EmployeeDtoAbstractModel>
//                {
//                    Id = id,
//                    ClientId = ClientId,
//                    Result = null,
//                    RecordCount = 0,
//                    Status = Status.Failed,
//                    Log = Trace.GetTraceLogs(),
//                    TimeConsumption = Trace.TimeConsumption(),
//                    Message = OperationFailed,
//                    StatusCode = StatusCodes.Status404NotFound
//                };
//        }


//        if (UserId != null) recordDb.Delete(UserId);
//        await UnitOfWorkService.CommitAsync();
//        return
//            new ResponseDto<EmployeeDtoAbstractModel>
//            {
//                Id = id,
//                ClientId = ClientId,
//                Result = null,
//                RecordCount = 1,
//                Status = Status.Success,
//                Log = Trace.GetTraceLogs(),
//                TimeConsumption = Trace.TimeConsumption(),
//                Message = OperationSuccessful,
//                StatusCode = StatusCodes.Status200OK
//            };

//    }
//    #endregion
//}