using Asp.Versioning;
using BSLayerSchool.BSInterfaces.InventoryManagement;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using InventoryMicroService.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.Inventory;
using SharedLibrary.Services.CustomFilters;

namespace InventoryMicroService.Controllers;

/// <summary>
/// Product Controller for Inventory management.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductController : ApiBaseController
{
    private readonly IBsProductContract _bsService;

    /// <summary>
    /// Product constructor
    /// </summary>
    /// <param name="bsService">Product business service</param>
    public ProductController(IBsProductContract bsService)
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
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<ProductDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<List<ProductDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, useCache);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtoModel"></param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<ProductDtoModel>> Save(ProductDtoModel dtoModel, bool useCache = true)
    {

        return await _bsService.AddAsync(dtoModel, useCache);
    }
    /// <summary>
    /// Update record
    /// </summary>
    /// <param name="id">record id which is used to update a record.</param>
    /// <param name="dtoModel">dto model</param>
    /// <param name="useCache">boolean value for redis cache.</param>
    /// <returns>status with ResponseDto</returns>
    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<ProductDtoModel>> Update(string id, [FromForm] ProductDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    /// <summary>
    /// deleting a record.
    /// </summary>
    /// <param name="id">id to delete</param>
    /// <param name="useCache">boolean value for redis cache.</param>
    /// <returns>status with ResponseDto</returns>
    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<ProductDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }

}
