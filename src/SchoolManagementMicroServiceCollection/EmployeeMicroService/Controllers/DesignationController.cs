
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
public class DesignationController : ApiBaseController
{
    private readonly IBsDesignationMasterContract _bsService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsService"></param>
    public DesignationController(IBsDesignationMasterContract bsService)
    {
        _bsService = bsService;
    }


    /// <summary>
    /// Get record for Designation model as per given id. Only active record will be return.
    /// </summary>
    /// <param name="id">Designation id</param>
    /// <param name="useCache"></param>
    /// <returns>Return single active Designation record which are available for given id.</returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.HR)]
    public async Task<ResponseDto<DesignationDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }


    /// <summary>
    /// Get all records for Designation model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active Designation records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.HR)]
    public async Task<ResponseDto<List<DesignationDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save Designation record.
    /// </summary>
    /// <param name="dtoModel">Designation Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created Designation record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.HR)]
    public async Task<ResponseDto<DesignationDtoModel>> Save(DesignationDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing Designation id.</param>
    /// <param name="dtoModel">Updated Designation dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.HR, RoleName.HR)]
    public async Task<ResponseDto<DesignationDtoModel>> Update(string id, DesignationDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given Designation model object.
    /// </summary>
    /// <param name="id">Existing Designation id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted Designation dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<DesignationDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
