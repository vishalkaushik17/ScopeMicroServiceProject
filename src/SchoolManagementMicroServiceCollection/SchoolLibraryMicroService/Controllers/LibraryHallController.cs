using Asp.Versioning;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.SchoolLibrary;
using SchoolLibraryMicroService.Controllers.Base;
using SharedLibrary.Services.CustomFilters;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;
namespace SchoolLibraryMicroService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class LibraryHallController : ApiBaseController
{
    private readonly IBsSchoolLibraryHallContract _bsService;
    public LibraryHallController(IBsSchoolLibraryHallContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }



    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<SchoolLibraryHallDtoModel>> Get(string id, bool UseCache = true)
    {
        return await _bsService.Get(id, UseCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<SchoolLibraryHallDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool UseCache = true)
    {
        return await _bsService.GetAll(pageNo, pageSize, UseCache);

    }


    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<SchoolLibraryHallDtoModel>> Save(SchoolLibraryHallDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.AddAsync(dtoModel, UseCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<SchoolLibraryHallDtoModel>> Update(string id, SchoolLibraryHallDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, UseCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<SchoolLibraryHallDtoModel>> Delete(string id, bool UseCache = true)
    {
        return await _bsService.DeleteAsync(id, UseCache);
    }

}
