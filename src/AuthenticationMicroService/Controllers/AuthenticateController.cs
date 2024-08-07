using BSAuthentication.BsInterface.AccountService;
using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelTemplates.RequestNResponse.Accounts;
using SharedLibrary.Services.CustomFilters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;
namespace AuthenticateService.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]

    public sealed class AuthenticateController : ControllerBase
    {
        private readonly IBsAccountService _accountService;
        private readonly IConfiguration _configuration;
        internal readonly bool _isTracingRequired;
        private readonly ITrace _trace;

        public IHttpContextAccessor _HttpContextAccessor { get; }

        public AuthenticateController(IBsAccountService accountService,
            IConfiguration configuration, ITrace trace, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _configuration = configuration;
            _isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
            _trace = trace;
            _HttpContextAccessor = httpContextAccessor;
        }



        [AllowAnonymous]
        [HttpGet("test")]

        public string test()
        {
            return "success!";
        }

        [AllowAnonymous]
        [HttpGet("ping")]

        public string ping()
        {
            return "success!";
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApplicationUserDtoModel>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _accountService.RefreshToken(refreshToken, ipAddress());
            await setTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailRequest model)
        {
            _accountService.VerifyEmail(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [AllowAnonymous]
        [HttpGet("account-confirmation")]
        public async Task<ResponseDto<ResponseOnActivation>> AccountConfirmation(string confirmId)
        {
            return await _accountService.AccountConfirmation(confirmId);

            //  return Ok(new { message = "Verification successful, you can now login" });
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ResponseDto<ApplicationUserDtoModel>> Login([FromBody] AuthenticateRequest model, [FromQuery] bool UseCache = true)
        {
            ResponseDto<ApplicationUserDtoModel> response = new ResponseDto<ApplicationUserDtoModel>(_HttpContextAccessor);
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess());
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Api - IP Address : ".MarkProcess(), ipAddress());

            response = await _accountService.Authenticate(model, ipAddress(), UseCache);

            if (response != null && response.Result.IsSuccess)
            {
                await setTokenCookie(response.Result.RefreshToken);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess());
            response.Log = _trace.GetTraceLogs("");
            return response;


        }


        //[HttpPost]
        //[Route("register1")]
        //[AllowAnonymous]
        //public async Task<ResponseDto<RegisterModel>> Register([FromBody] RegisterModel model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //    {

        //        return await Task.Run(() => new ResponseDto<RegisterModel>

        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            //Token = null,
        //            Status = Status.Failed,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = UserAlreadyRegistered + model.Username
        //        });
        //    }
        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    //Add default Username to Role Admin    
        //    // first we create Admin rool    
        //    var role = new IdentityRole
        //    {
        //        Name = "Admin"
        //    };

        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        Trace.TraceMe(Method, IsTracingRequired, OperationFailed);

        //        return await Task.Run(() => new ResponseDto<RegisterModel>
        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            //Token = null,
        //            Status = Status.Failed,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = OperationFailed
        //        });
        //    }

        //    var roleExists = await _roleManager.FindByNameAsync(role.Name);
        //    if (roleExists == null)
        //    {
        //        var rr = new UserRoles("default");
        //        rr.UserId = "DEFAULT";
        //        rr.ModifiedOn = null;
        //        //rr.CompanyId=
        //        //await _roleManager.CreateAsync(role);
        //    }

        //    result = await _userManager.AddToRoleAsync(user, role.Name);
        //    if (result.Succeeded)
        //    {

        //        return await Task.Run(() => new ResponseDto<RegisterModel>
        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status201Created,
        //            //Token = null,
        //            Status = Status.Success,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = OperationSuccessful
        //        });
        //    }
        //    else
        //    {
        //        Trace.TraceMe(Method, IsTracingRequired, OperationFailed.ToCss());

        //        return await Task.Run(() => new ResponseDto<RegisterModel>
        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            //Token = null,
        //            Status = Status.Failed,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = OperationFailed
        //        });
        //    }
        //}

        //[HttpPost]
        //[Route("register-admin")]
        //[AllowAnonymous]
        //public async Task<ResponseDto<RegisterModel>> RegisterAdmin([FromBody] RegisterModel model)
        //{

        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //    {
        //        Trace.TraceMe(Method, IsTracingRequired, OperationFailed.ToCss());
        //        return await Task.Run(() => new ResponseDto<RegisterModel>
        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            //Token = null,
        //            Status = Status.Failed,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = UserExists
        //        });
        //    }

        //    ApplicationUser user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        CompanyId = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        Trace.TraceMe(Method, IsTracingRequired, OperationFailed.ToCss());
        //        return await Task.Run(() => new ResponseDto<RegisterModel>
        //        {
        //            Result = model,
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            //Token = null,
        //            Status = Status.Failed,
        //            Log = Trace.GetTraceLogs(),
        //            TimeConsumption = Trace.TimeConsumption(),
        //            Message = InternalServerError
        //        });
        //    }

        //    //if (!await _roleManager.RoleExistsAsync(RolesForUsers.Admin))

        //    //    await _roleManager.CreateAsync(new UserRoles(RolesForUsers.Admin));
        //    //if (!await _roleManager.RoleExistsAsync(RolesForUsers.User))
        //    //    await _roleManager.CreateAsync(new IdentityRole(RolesForUsers.User));

        //    //if (await _roleManager.RoleExistsAsync(RolesForUsers.Admin))
        //    //{
        //    //    await _userManager.AddToRoleAsync(user, RolesForUsers.Admin);
        //    //}
        //    //if (await _roleManager.RoleExistsAsync(RolesForUsers.Admin))
        //    //{
        //    //    await _userManager.AddToRoleAsync(user, RolesForUsers.User);
        //    //}
        //    Trace.TraceMe(Method, IsTracingRequired, OperationSuccessful.ToCss());
        //    await _unitOfWorkService.CommitAsync();
        //    return await Task.Run(() => new ResponseDto<RegisterModel>
        //    {
        //        Result = model,
        //        StatusCode = StatusCodes.Status201Created,
        //        //Token = null,
        //        Status = Status.Success,
        //        Log = Trace.GetTraceLogs(),
        //        TimeConsumption = Trace.TimeConsumption(),
        //        Message = UserCreated
        //    });
        //    //return Ok(new ResponseDto { RecordStatus = Success, Message = UserCreated });
        //}

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration[IssuerSigningKey]));
            return new JwtSecurityToken(
                issuer: _configuration[ValidIssuer],
                audience: _configuration[ValidAudience],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                    SecurityAlgorithms.HmacSha256)
                );
        }


        // helper methods

        private async Task setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            await Task.Run(() => Response.Cookies.Append("refreshToken", token, cookieOptions));
        }

        private string ipAddress()
        {
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //if (Request.Headers.ContainsKey("X-Forwarded-For"))
            //    return Request.Headers["X-Forwarded-For"];
            //else
            //    return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
