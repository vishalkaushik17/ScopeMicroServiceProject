
using Asp.Versioning;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.AppConfig;
using ModelTemplates.DtoModels.SchoolLibrary;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;

namespace CodingCompanyService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApplicationHostingController : ControllerBase// ApiBaseController
{
    private readonly IBsApplicationHostContract _bsService;
    public ApplicationHostingController(IBsApplicationHostContract bsService)
    {
        _bsService = bsService;
    }

    [HttpGet, AllowAnonymous]
    [Route("getall")]
    public async Task<ResponseDto<List<ApplicationHostMasterDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool UseCache = true)
    {
        return await _bsService.GetAll(pageNo, pageSize, UseCache);
    }

    [HttpGet, AllowAnonymous]
    [Route("get")]
    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> Get(string id, bool UseCache = true)
    {
        return await _bsService.Get(id, UseCache);
    }

    [HttpGet, AllowAnonymous]
    [Route("ping")]
    public string ping()
    {
        return "HostTable- Success!";
    }

    [HttpPost, AllowAnonymous]
    [Route("add")]
    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> Add(ApplicationHostMasterDtoModel dtoModel)
    {
        return await _bsService.AddAsync(dtoModel);
    }

    [HttpPost, AllowAnonymous]
    [Route("update")]
    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> Update(string id, ApplicationHostMasterDtoModel dtoTemplate)
    {
        return await _bsService.UpdateAsync(id, dtoTemplate);
    }

    [HttpGet, AllowAnonymous]
    [Route("remove")]
    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> Delete(string id)
    {
        return await _bsService.DeleteAsync(id);
    }
}