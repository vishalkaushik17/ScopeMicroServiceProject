
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
public class DepartmentController : ApiBaseController
{
    private readonly IBsDepartmentContract _bsService;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsService"></param>
    public DepartmentController(IBsDepartmentContract bsService)
    {
        _bsService = bsService;
    }


    /// <summary>
    /// Get record for Department model as per given Id. Only active record will be return.
    /// </summary>
    /// <param name="id">Department Id</param>
    /// <param name="useCache"></param>
    /// <returns>Return single active Department record which are available for given Id.</returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.AsstHR, RoleName.Master)]
    public async Task<ResponseDto<DepartmentDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.GetAsync(id, useCache);
    }


    /// <summary>
    /// Get all records for Department model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active Department records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.AsstHR, RoleName.Master)]
    public async Task<ResponseDto<List<DepartmentDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAllAsync(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save Department record.
    /// </summary>
    /// <param name="dtoModel">Department Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created Department record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.AsstHR, RoleName.Master)]
    public async Task<ResponseDto<DepartmentDtoModel>> Save(DepartmentDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing Department Id.</param>
    /// <param name="dtoModel">Updated Department dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.AsstHR, RoleName.Master)]
    public async Task<ResponseDto<DepartmentDtoModel>> Update(string id, DepartmentDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given Department model object.
    /// </summary>
    /// <param name="id">Existing Department Id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted Department dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.AsstHR, RoleName.Master)]
    public async Task<ResponseDto<DepartmentDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
