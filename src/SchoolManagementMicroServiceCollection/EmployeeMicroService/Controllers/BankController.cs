
using Asp.Versioning;
using BSLayerSchool.BSInterfaces.EmployeeContracts;
using EmployeeMicroService.Controllers.Base;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.Employee;
using SharedLibrary.Services.CustomFilters;

namespace EmployeeMicroService.Controllers;
/// <summary>
/// This controller is defined for operation on employee model.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BankController : ApiBaseController
{
    private readonly IBsBankMasterContract _bsService;

    /// <summary>
    /// Parameterized Constructor for controller.
    /// </summary>
    /// <param name="bsService"></param>
    public BankController(IBsBankMasterContract bsService)
    {
        _bsService = bsService;
    }


    /// <summary>
    /// Get record for Bank model as per given Id. Only active record will be return.
    /// </summary>
    /// <param name="id">Bank Id</param>
    /// <param name="useCache"></param>
    /// <returns>Return single active Bank record which are available for given Id.</returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<BankDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }


    /// <summary>
    /// Get all records for Bank model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active Bank records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<BankDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save Bank record.
    /// </summary>
    /// <param name="dtoModel">Bank Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created Bank record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<BankDtoModel>> Save(BankDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing Bank Id.</param>
    /// <param name="dtoModel">Updated Bank dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<BankDtoModel>> Update(string id, BankDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given Bank model object.
    /// </summary>
    /// <param name="id">Existing Bank Id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted Bank dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<BankDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
