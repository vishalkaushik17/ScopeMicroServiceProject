using Asp.Versioning;
using BSCodingCompany.BSInterfaces.DemoRequest;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.Company;
using SharedLibrary.Services.CustomFilters;
using CodingCompanyService.Controllers.Base;
namespace CodingCompanyService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class DemoRequestController : ApiBaseController
    {

        public readonly IBsDemoRequestContract _demoRequestContract;
        public DemoRequestController(IBsDemoRequestContract demoRequestContract, ITrace trace) : base(trace)
        {
            _demoRequestContract = demoRequestContract;
        }

        [HttpPost, AllowAnonymous]
        [Route("requestdemo")]

        //This controller will process demo request for the school/institution
        public async Task<ResponseDto<DemoRequestDtoModel>> GenerateRequestForDemo(DemoRequestDtoModel dto)
        {
            //All process is done by Unitofwork.DemoRequestContract
            //It will also send email to school who requested demo.
            return await _demoRequestContract.GenerateDemoRequest(dto);
        }




        [HttpPost, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [Route("activatedemo")]

        //This controller will process demo request for the school/institution
        public async Task<ResponseDto<CompanyMasterDtoModel>> ActivateDemo(string demoId, string hostId, string? customSuffixDomain, string? refCode)
        {

            //All process is done by Unitofwork.DemoRequestContract
            //It will also send email to school who requested demo.
            return await _demoRequestContract.ActivateDemo(demoId, hostId, customSuffixDomain, refCode);
            //return await _demoRequestContract.ActivateDemo(demoId, customSuffixDomain, _UserId, hostId, Request.Headers["origin"]);
        }


        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous] //change to rolebase
        [Route("getall")]
        public async Task<ResponseDto<List<DemoRequestDtoModel>>> GetAll(bool isActive = false)
        {
            return await _demoRequestContract.GetAll(isActive);

        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbyany")]
        public async Task<ResponseDto<DemoRequestDtoModel>> Get(string? id = "", string? refCode = "", string? website = "", string? contactNo = "", string? email = "")
        {
            return await _demoRequestContract.GetByAny(id, refCode, website, contactNo, email);

        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbywebsite")]
        public async Task<ResponseDto<DemoRequestDtoModel>> GetByWebSite(string website)
        {
            return await _demoRequestContract.GetByWebsite(website);

        }



        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbyrefcode")]
        //public async Task<ActionResult<ResponseDto<DemoRequestDtoModel>>> GetByRefCode(string refCode)
        public async Task<ActionResult<ResponseDto<DemoRequestDtoModel>>> GetByRefCode(string refCode)
        {
            return await _demoRequestContract.GetByReferenceCode(refCode);

            //var result = await _UnitOfWorkService.DemoRequestContract.GetByReferenceCode(refCode);
            //if (result.StatusCode == StatusCodes.Status404NotFound)
            //{
            //    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, result);

            //}

            //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, result);
        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbyemail")]
        public async Task<ResponseDto<DemoRequestDtoModel>> GetByEmail(string email)
        {
            return await _demoRequestContract.GetByEmailId(email);

        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbyid")]
        public async Task<ResponseDto<DemoRequestDtoModel>> GetById(string id)
        {
            return await _demoRequestContract.GetDemoRequestById(id);

        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getbycontactno")]
        public async Task<ResponseDto<DemoRequestDtoModel>> GetByContactNo(string contactNo)
        {
            return await _demoRequestContract.GetByContactNo(contactNo);

        }

        //[HttpGet, CustomAuthorize(RoleName.Administrator, RoleName.Master)]
        [HttpGet, AllowAnonymous]
        [Route("getallbyreferencecode")]
        public async Task<ResponseDto<List<DemoRequestDtoModel>>> GetAllByReferenceCode(string referenceNo)
        {
            return await _demoRequestContract.GetAllByReferenceCode(referenceNo);

        }
    }
}

