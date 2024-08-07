
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
public class DegreeController : ApiBaseController
{
    private readonly IBsDegreeContract _bsService;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsService"></param>
    
    public DegreeController(IBsDegreeContract bsService)
    {
        _bsService = bsService;
    }


    /// <summary>
    /// Get record for Degree model as per given id. Only active record will be return.
    /// </summary>
    /// <param name="id">Degree Id</param>
    /// <param name="useCache"></param>
    /// <returns>Return single active Degree record which are available for given id.</returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<DegreeDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }


    /// <summary>
    /// Get all records for Degree model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active Degree records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<DegreeDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save Degree record.
    /// </summary>
    /// <param name="dtoModel">Degree Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created Degree record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<DegreeDtoModel>> Save(DegreeDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing Degree Id.</param>
    /// <param name="dtoModel">Updated Degree dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<DegreeDtoModel>> Update(string id, DegreeDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given Degree model object.
    /// </summary>
    /// <param name="id">Existing Degree Id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted Degree dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<DegreeDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
