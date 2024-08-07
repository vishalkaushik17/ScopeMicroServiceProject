
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
public class EmployeeQualificationController : ApiBaseController
{
    private readonly IBsEmployeeQualificationContract _bsService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsService"></param>
    public EmployeeQualificationController(IBsEmployeeQualificationContract bsService)
    {
        _bsService = bsService;
    }


    /// <summary>
    /// Get record for EmployeeQualification model as per given id. Only active record will be return.
    /// </summary>
    /// <param name="id">EmployeeQualification id</param>
    /// <param name="useCache"></param>
    /// <returns>Return single active EmployeeQualification record which are available for given id.</returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<EmployeeQualificationDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }


    /// <summary>
    /// Get all records for EmployeeQualification model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active EmployeeQualification records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<EmployeeQualificationDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save EmployeeQualification record.
    /// </summary>
    /// <param name="dtoModel">EmployeeQualification Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created EmployeeQualification record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<EmployeeQualificationDtoModel>> Save(EmployeeQualificationDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing EmployeeQualification id.</param>
    /// <param name="dtoModel">Updated EmployeeQualification dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<EmployeeQualificationDtoModel>> Update(string id, EmployeeQualificationDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given EmployeeQualification model object.
    /// </summary>
    /// <param name="id">Existing EmployeeQualification id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted EmployeeQualification dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<EmployeeQualificationDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
