using Asp.Versioning;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.SchoolLibrary;
using SchoolLibraryMicroService.Controllers.Base;

//using ModelTemplates.EntityModels.Application;

using SharedLibrary.Services.CustomFilters;

namespace SchoolLibraryMicroService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthorController : ApiBaseController
    {
        private readonly IBsLibraryAuthorContract _bsService;

        public AuthorController(IBsLibraryAuthorContract bsService, ITrace trace) : base(trace)
        {
            _bsService = bsService;
        }

        [HttpGet]
        [Route("Get")]
        [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
        public async Task<ResponseDto<LibraryAuthorDtoModel>> Get(string id, bool UseCache = true)
        {
            return await _bsService.Get(id, UseCache);
        }

        [HttpGet]
        [Route("GetAll")]
        [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
        public async Task<ResponseDto<List<LibraryAuthorDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool UseCache = true)
        {

            return await _bsService.GetAll(pageNo, pageSize, UseCache);
        }

        [HttpPost]
        [Route("Save")]
        //[CustomAuthorize(RoleName.ErpAdmin)]
        public async Task<ResponseDto<LibraryAuthorDtoModel>> Save(LibraryAuthorDtoModel dtoModel, bool UseCache = true)
        {
            return await _bsService.AddAsync(dtoModel, UseCache);
        }

        [HttpPost]
        [Route("Update")]
        [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
        public async Task<ResponseDto<LibraryAuthorDtoModel>> Update(string id, LibraryAuthorDtoModel dtoModel, bool UseCache = true)
        {
            return await _bsService.UpdateAsync(id, dtoModel, UseCache);
        }

        [HttpGet]
        [Route("Delete")]
        [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
        public async Task<ResponseDto<LibraryAuthorDtoModel>> Delete(string id, bool UseCache = true)
        {
            return await _bsService.DeleteAsync(id, UseCache);
        }

    }
}
