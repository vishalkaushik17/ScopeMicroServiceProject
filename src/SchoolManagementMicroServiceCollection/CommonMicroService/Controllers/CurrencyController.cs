using Asp.Versioning;
using BSLayerSchool.BSInterfaces.CommonContracts;
using CommonMicroService.Controllers.Base;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.CommonDtoModels;
using SharedLibrary.Services.CustomFilters;

namespace CommonMicroService.Controllers;
/// <summary>
/// This controller is defined for operation on currency model.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CurrencyController : ApiBaseController
{
    private readonly IBsCurrencyContract _bsService;

    
    /// <summary>
    /// Currency controller
    /// </summary>
    /// <param name="bsService"></param>
    /// <param name="trace"></param>
    public CurrencyController(IBsCurrencyContract bsService, ITrace trace) : base()
    {
        _bsService = bsService;
    }


    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<CurrencyDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }


    /// <summary>
    /// Get all records for currency model which are available in database.  Only active record will be return.
    /// </summary>
    /// <param name="pageNo">Get records which are available on that Page no</param>
    /// <param name="pageSize">How many records are defined on one page.</param>
    /// <param name="useCache"></param>
    /// <returns>Return active currency records.</returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<CurrencyDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// Save currency record.
    /// </summary>
    /// <param name="dtoModel">Currency Dto Model object.</param>
    /// <param name="useCache"></param>
    /// <returns>Return newly created currency record.</returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<CurrencyDtoModel>> Save(CurrencyDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    /// <summary>
    /// Update existing record as per given id and newly updated model object.
    /// </summary>
    /// <param name="id">Existing currency Id.</param>
    /// <param name="dtoModel">Updated currency dto model object.</param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<CurrencyDtoModel>> Update(string id, CurrencyDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// Mark deleted status on given currency model object.
    /// </summary>
    /// <param name="id">Existing currency Id.</param>
    /// <param name="useCache"></param>
    /// <returns>Return deleted currency dto object.</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<CurrencyDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }



}
