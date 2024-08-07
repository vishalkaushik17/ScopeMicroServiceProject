//using CommonMicroService.Controllers.Base;

using Asp.Versioning;
using BSLayerSchool.BSInterfaces.CommonContracts;
using CommonMicroService.Controllers.Base;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.Inventory;
using SharedLibrary.Services.CustomFilters;

namespace CommonMicroService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VendorController : ApiBaseController
{
    private readonly IBsVendorContract _bsService;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsService"></param>
    /// <param name="trace"></param>
    public VendorController(IBsVendorContract bsService, ITrace trace) : base()
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<VendorDtoModel>> Get(string id, bool UseCache = true)
    {
        return await _bsService.Get(id, UseCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Master, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<VendorDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool UseCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize, UseCache);
    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<VendorDtoModel>> Save(VendorDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.AddAsync(dtoModel, UseCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<VendorDtoModel>> Update(string id, VendorDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, UseCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<VendorDtoModel>> Delete(string id, bool UseCache = true)
    {
        return await _bsService.DeleteAsync(id, UseCache);
    }



}
