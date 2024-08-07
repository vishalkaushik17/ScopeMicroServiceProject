using AutoMapper;
using BSAuthentication.BaseClass;
using BSAuthentication.BsInterface.AccountService;
using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Core.Contracts.CommonServices;
using DataBaseServices.Core.Contracts.UsersAndRoles;
using DataBaseServices.LayerRepository;
using DataBaseServices.LayerRepository.Services;
using DataCacheLayer.CacheRepositories.Interfaces;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EncryptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.RequestNResponse.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;
namespace BSAuthentication.BsServices.AccountService;

public sealed class BsAccountService : BaseBusinessLayer, IBsAccountService
{

	private readonly IJwtUtils _jwtUtils;
	private readonly IMapper mapper;

	private readonly IEmailService _emailService;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ITrace trace;
	private readonly ApplicationUserDtoModel _ApplicationUserDtoModel = new ApplicationUserDtoModel();
	private readonly IDataLayerApplicationHostContract _dataLayerApplicationHostContract;
	private readonly IDataLayerCompanyMasterVsDbHost _dataLayerCompanyMasterVsDbHost;
	private readonly IDataLayerAccountService _dataLayerAccountService;
	private readonly IDataLayerNewDBService _dataLayerNewDBService;
	private readonly bool _isTracingRequired;
	private readonly IDataLayerNewDBService _dlNewDBService;
	private readonly ICacheContract cache;
	private readonly IDataLayerUserContract _dlUsers;
	private string? dbStringForClientConnection = string.Empty;
	private List<DbConnectionStringRecord>? domainList;
	public ApplicationDbContext _dBContext { get; set; }

	public BsAccountService(IDataLayerApplicationHostContract dataLayerApplicationHostContract,
		IDataLayerCompanyMasterVsDbHost dataLayerCompanyMasterVsDbHost,
		IDataLayerAccountService dataLayerAccountService,
		IDataLayerNewDBService dataLayerNewDBService,
		IJwtUtils jwtUtils,
		IMapper mapper,
		IEmailService emailService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ITrace trace,
		IDataLayerNewDBService dlNewDBService, ICacheContract cache, ApplicationDbContext applicationDbContext, IDataLayerUserContract users)
		: base(mapper, httpContextAccessor, trace)
	{
		_jwtUtils = jwtUtils;
		this.mapper = mapper;
		_emailService = emailService;
		_userManager = userManager;
		this._httpContextAccessor = httpContextAccessor;
		this.trace = trace;
		_dlNewDBService = dlNewDBService;
		this.cache = cache;
		_dBContext = applicationDbContext;
		_dlUsers = users;
		_dataLayerApplicationHostContract = dataLayerApplicationHostContract;
		_dataLayerCompanyMasterVsDbHost = dataLayerCompanyMasterVsDbHost;
		_dataLayerAccountService = dataLayerAccountService;
		_dataLayerNewDBService = dataLayerNewDBService;
		_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");

		_cacheData = cache;
		domainList = new List<DbConnectionStringRecord>();
	}

	public async Task<ResponseDto<ApplicationUserDtoModel>> Authenticate(AuthenticateRequest model, string ipAddress, bool UseCache = true)
	{
		




		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
	   _isTracingRequired, this.NameOfClass().MarkProcess(), "Authenticate Process started from : " + this.NameOfClass() + "".GetCurrentLineNo());
		ResponseDto<ApplicationUserDtoModel>? responseDto = new ResponseDto<ApplicationUserDtoModel>(base._httpContextAccessor);
		//var watch = System.Diagnostics.Stopwatch.Startnew(_httpContextAccessor);


		//add migration if pending
		await _dataLayerNewDBService.ApplyMigration();
		//_dBContext.Database.Migrate();
		var users = _httpContextAccessor.HttpContext.GetContextItemAsJson<List<ApplicationUser>>(ContextKeys.Users);
		if (users == null)
		{
			users = await _cacheData.ReadFromCacheAsync<ApplicationUser>(string.Format(CacheKeys.Users, UserIdentity.GetClientId(base._httpContextAccessor.HttpContext)), 7, true);
		}
		//users = await _cacheData.ReadFromCacheAsync<ApplicationUser>(string.Format(CacheKeys.Users, UserIdentity.GetClientId(_httpContextAccessor.HttpContext)), 7, true);

		if (users == null || users.Count == 0)
		{

			//Getting system preference from current company.
			//users = await _dBContext.User.Include(m => m.Company).Where(m => m.CompanyId == UserIdentity.GetClientId(_httpContextAccessor.HttpContext)).ToListAsync();

			//await _dlUsers.SetDbContext(new ApplicationDbContext(_dlUsers.GetDbContext().Result._Options, _httpContextAccessor, _trace));
			users = await _dlUsers.GetUsersByClientId(UserIdentity.GetClientId(base._httpContextAccessor.HttpContext));
			if (users == null || users.Count == 0)
			{
				_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "No users!".MarkProcess(), "Re-initializing connection!");

				//await _dlUsers.SetDbContext(await _dataLayerNewDBService.GetDbContext());
				//users = await _dBContext.User.Include(m => m.Company).Where(m => m.CompanyId == UserIdentity.GetClientId(_httpContextAccessor.HttpContext)).ToListAsync();
				users = await _dlUsers.GetUsersByClientId(UserIdentity.GetClientId(base._httpContextAccessor.HttpContext));
				if (users == null || users.Count == 0)
				{
					_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "No users!".MarkProcess(), "Connection re-established, check logs for more information!");
				}
			}
			if (users != null && users.Count != 0)
			{
				_httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.Users, users);
				_expirationTime = _applicationSettings.ModuleCacheSettings.Users.GetKeyLifeForCacheStorage();
				await _cacheData.AddCacheAsync(string.Format(CacheKeys.Users, UserIdentity.GetClientId(base._httpContextAccessor.HttpContext)), users, _expirationTime);
			}

		}
		var user = users?.FirstOrDefault(x => x.UserName == model.Username);

		//var user = await _dataLayerAccountService.GetUserWithCompanyData(model.Username, model.Username);

		if (user == null)
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "No user!".MarkProcess(), "Invalid credentials!");

			_ApplicationUserDtoModel.Errors.Add("Invalid credentials!");
			responseDto.Message = CommonMessages.InvalidCredentials;
			responseDto.StatusCode = StatusCodes.Status401Unauthorized;

			_ApplicationUserDtoModel.IsVerified = false;
			_ApplicationUserDtoModel.IsSuccess = false;
			responseDto.Result = _ApplicationUserDtoModel;
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess());
			//responseDto.Log = _trace.GetTraceLogs("");
			return responseDto;
		}

		if (!VerifyHashedPassword(user.PasswordHash, model.Password))
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "VerifyHashedPassword!".MarkProcess(), "Invalid credentials!");
			_ApplicationUserDtoModel.Errors.Add("Invalid credentials!");
			responseDto.Message = InvalidCredentials;
			responseDto.StatusCode = StatusCodes.Status401Unauthorized;
			_ApplicationUserDtoModel.IsVerified = false;
			_ApplicationUserDtoModel.IsSuccess = false;
			responseDto.Result = _ApplicationUserDtoModel;
			//responseDto.Log = _trace.GetTraceLogs("");
			return responseDto;
		}

		// authentication successful so generate jwt and refresh tokens
		var userRoles = _userManager.GetRolesAsync(user).Result;

		var sessionId = base._httpContextAccessor.HttpContext?.GetHeader(ContextKeys.TokenSessionId);
		var scopeId = base._httpContextAccessor.HttpContext?.GetHeader(ContextKeys.TokenScopeId);

		var jwtToken = await _jwtUtils.GenerateJwtToken(user, userRoles, sessionId, scopeId);

		var refreshToken = await _jwtUtils.GenerateRefreshToken(user.Id, user.CompanyId);
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "User Roles, token, refresh token fetched".MarkProcess());

		refreshToken.CreatedByIp = ipAddress;
		refreshToken.CompanyId = user.CompanyId;
		refreshToken.CompanyMasterEntityModel = null;
		refreshToken.User = null;

		await _dBContext.RefreshTokens.AddAsync(refreshToken);

		await _dBContext.SaveChangesAsync();

		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Available Roles for logged in user :{ExtensionMethods.ToString(userRoles)} ".MarkInformation());
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "AddRefreshToken".MarkProcess());

		// remove old refresh tokens from user
		removeOldRefreshTokens(user);
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "removeOldRefreshTokens".MarkProcess());

		_ApplicationUserDtoModel.Roles = (List<string>)userRoles;
		//userRoles.ToList().ForEach((role) =>
		//{
		//    response.Roles.Add(role.ToString());
		//});
		_ApplicationUserDtoModel.JwtToken = jwtToken;
		_ApplicationUserDtoModel.RefreshToken = refreshToken.Token;
		_ApplicationUserDtoModel.IsSuccess = true;
		_ApplicationUserDtoModel.IsVerified = true;
		_ApplicationUserDtoModel.UserId = user.Id;
		_ApplicationUserDtoModel.UserName = user.UserName;
		_ApplicationUserDtoModel.UserEmail = user.Email;
		_ApplicationUserDtoModel.PersonalEmailId = user.PersonalEmailId;
		_ApplicationUserDtoModel.ClientId = user.CompanyId;
		_ApplicationUserDtoModel.ClientName = user.Company.Name;
		_ApplicationUserDtoModel.ClientEmail = user.Company.Email;
		_ApplicationUserDtoModel.CompanyTypeId = user.Company.CompanyTypeId;
		_ApplicationUserDtoModel.WebSite = user.Company.Website;
		_ApplicationUserDtoModel.FirstName = user.FirstName;
		_ApplicationUserDtoModel.LastName = user.LastName;
		_ApplicationUserDtoModel.CreatedOn = DateTime.Now;
		_ApplicationUserDtoModel.TokenSessionId = sessionId;
		_ApplicationUserDtoModel.TokenScopeId = scopeId;

		responseDto.Result = _ApplicationUserDtoModel;

		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Generating Authenticate Response : ".MarkProcess(), nameof(_ApplicationUserDtoModel));
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "User : ".MarkProcess(), _ApplicationUserDtoModel.UserName);

		responseDto.Id = user.Id;
		responseDto.UserName = user.UserName;
		responseDto.Status = Status.Success;
		responseDto.ClientId = user.CompanyId;
		responseDto.ClientName = user.Company.Name;
		responseDto.StatusCode = StatusCodes.Status302Found;
		responseDto.Message = CommonMessages.OperationSuccessful;
		responseDto.RecordCount = 1;




		////adding default system preferences
		//var systemPreferences = await _cacheData.ReadFromCacheAsync<SystemPreferenceForSession>(nameof(SystemPreferencesModel), 0, true);
		//if (systemPreferences == null)
		//{
		//    systemPreferences = await _dBContext.SystemPreferences
		//    .Where(m => m.RecordStatus == GenericFunction.Enums.EnumRecordStatus.Active)
		//    .Select(m => new SystemPreferenceForSession
		//    {
		//        PreferenceName = m.PreferenceName,
		//        Value = m.CustomValue.ToLower() == "n/a" || m.CustomValue == "0" ? m.DefaultValue : m.CustomValue,
		//        ModuleName = m.ModuleName,
		//        ValueType = m.ValueType,

		//    }).ToListAsync();
		//    _expirationTime = DateTimeOffset.Now.AddMinutes(_applicationSettings.ModuleCacheSettings.SystemPreferences);
		//    await _cacheData.AddCacheAsync<List<SystemPreferenceForSession>>(_cacheData.GenerateSpecificKeyForCache(nameof(SystemPreferencesModel), user.CompanyId), systemPreferences, _expirationTime);
		//}
		//_httpContextAccessor?.HttpContext?.Items.Add($"SystemPreferences-{user.CompanyId}", systemPreferences);




		
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "All Good!".MarkProcess(), "Process ended of : " + this.NameOfClass());
		//responseDto.TimeConsumption = 0;
		//responseDto.Log = _trace.GetTraceLogs("");
		return responseDto;
	}

	public async Task<ApplicationUserDtoModel> RefreshToken(string token, string ipAddress)
	{
		var user = await getAccountByRefreshToken(token);
		var userRoles = _userManager.GetRolesAsync(user).Result;
		var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
		if (refreshToken == null)
			throw new AppException("Invalid token");
		if (refreshToken.IsRevoked)
		{
			// revoke all descendant tokens in case this token has been compromised
			revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
			await _dataLayerAccountService.UpdateUser(user);
		}

		if (!refreshToken.IsActive)
			throw new AppException("Invalid token");

		// replace old refresh token with a new one (rotate token)
		var newRefreshToken = await rotateRefreshToken(refreshToken, ipAddress, user.Id);
		user.RefreshTokens.Add(newRefreshToken);

		// remove old refresh tokens from user
		removeOldRefreshTokens(user);

		// save changes to db
		await _dataLayerAccountService.UpdateUser(user);
		var sessionId = base._httpContextAccessor.HttpContext?.GetHeader(ContextKeys.TokenSessionId);

		var scopeId = base._httpContextAccessor.HttpContext?.GetHeader(ContextKeys.TokenScopeId);
		// generate new jwt
		var jwtToken = await _jwtUtils.GenerateJwtToken(user, userRoles, sessionId, scopeId);

		// return data in authenticate response object
		var response = _mapper.Map<ApplicationUserDtoModel>(user);
		response.JwtToken = jwtToken;
		response.RefreshToken = newRefreshToken.Token;
		return response;
	}

	//public void RevokeToken(string token, string ipAddress)
	//{
	//    var account = getAccountByRefreshToken(token);
	//    var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

	//    if (!refreshToken.IsActive)
	//        throw new AppException("Invalid token");

	//    // revoke token and save
	//    revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
	//    context.Update(account);
	//    context.SaveChanges();
	//}

	public async Task Register(RegisterRequest model, string origin)
	{
		// validate
		var user = await _dataLayerAccountService.FindUser(model.Email, model.Username);

		if (user != null)
		{
			// send already registered error in email to prevent user enumeration
			sendAlreadyRegisteredEmail(user.Email, origin);
			return;
		}

		// map model to new user object
		var account = _mapper.Map<ApplicationUser>(model);

		// first registered user is an admin

		//account.Role = isFirstAccount ? RoleName.Administrator : RoleName.User;
		account.CreatedOn = DateTime.UtcNow;
		account.VerificationToken = await generateVerificationToken();

		// hash password
		//account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

		var result = _dataLayerAccountService.CreateUser(account, model.Password).Result;
		if (result.Succeeded)
			sendVerificationEmail(account, origin);
	}

	public async Task VerifyEmail(string token)
	{
		var user = await _dataLayerAccountService.VerifyEmail(token);
		if (user == null)
			throw new AppException("Verification failed");

		user.Verified = DateTime.UtcNow;
		user.VerificationToken = null;

		await _dataLayerAccountService.UpdateUser(user);

	}

	public async Task<ResponseDto<ResponseOnActivation>> AccountConfirmation(string confirmId)
	{

		ResponseDto<ResponseOnActivation> responseDto = new(base._httpContextAccessor);
		var confirmLinkDecryptData = IEncryptionService.Decrypt(confirmId, "CODINGCOMPANY.IN");
		//var activationResponse = JsonConvert.DeserializeObject<ActivationLinkResponseObjects>(confirmLinkDecryptData);
		var activationResponse = confirmLinkDecryptData.FromJsonToObject<ResponseReturn>();

		if (activationResponse == null)
		{
			responseDto.Message = "Invalid link provided!";
			return responseDto;
		}

		if (activationResponse.ExpiredDate < DateTime.Now)
		{
			responseDto.Result.IsVerified = false;
			responseDto.Message = "Link is expired!";
		}


		//Get hosting record form main database
		var hostRecord = await _dataLayerApplicationHostContract.GetAsync(activationResponse.HostId);

		if (hostRecord == null)
		{
			responseDto.Message = "Invalid link provided!";
			return responseDto;
		}


		var connectionString = IEncryptionService.Decrypt(hostRecord.ConnectionString, hostRecord.HashString);

		if (base._httpContextAccessor.HttpContext != null)
		{
			base._httpContextAccessor.HttpContext.SetHeader(ContextKeys.dbConnectionString, connectionString);
		}
		else
		{
			responseDto = new(base._httpContextAccessor)
			{

				Result = null,
				RecordCount = 0,
				Message = "New database can not be created due to httpcontext is missing!",
				MessageType = MessageType.DefaultRecordWarning,
				StatusCode = CustomStatusCodes.RequiredParameterMissing,
				Status = Status.Failed,
			};
			return await Task.Run(() => responseDto);
		}

		//Generate new database link
		var dbConnectionStringLink = _dataLayerNewDBService.GetNewConnection(base._httpContextAccessor);

		var accountConfirm = await _dataLayerNewDBService.AccountConfirmation(dbConnectionStringLink, activationResponse.HostId, activationResponse.Id);

		if (accountConfirm == null)
		{
			responseDto.Message = "Account is already confirmed!";
			return responseDto;

		}

		var user = await _dataLayerNewDBService.FindUser(dbConnectionStringLink, activationResponse.UserId);

		if (user == null)
		{
			responseDto.Message = "User is not registered with us";
			return responseDto;
		}


		user.ConfirmEmail();
		accountConfirm.ConfirmUpdate();

		await dbConnectionStringLink.SaveChangesAsync(true);



		responseDto.Result = new ResponseOnActivation() { IsVerified = true, UserId = user.Id };
		responseDto.Message = CommonMessages.OperationSuccessful;
		responseDto.RecordCount = 1;
		responseDto.UserName = user.UserName;
		responseDto.MessageType = MessageType.Information;
		responseDto.StatusCode = StatusCodes.Status200OK;
		responseDto.Status = Status.Success;

		return responseDto;

	}

	public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
	{
		var user = await _dataLayerAccountService.FindUser(model.Email, model.Email);


		// always return ok response to prevent email enumeration
		if (user == null) return;

		// create reset token that expires after 1 day
		user.ResetToken = await generateResetToken();
		user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

		await _dataLayerAccountService.UpdateUser(user);

		// send email
		sendPasswordResetEmail(user, origin);
	}

	//public void ValidateResetToken(ValidateResetTokenRequest model)
	//{
	//    getAccountByResetToken(model.Token);
	//}

	//public async void ResetPassword(ResetPasswordRequest model)
	//{
	//    var user = getAccountByResetToken(model.Token);

	//    // update password and remove reset token
	//    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
	//    user.PasswordReset = DateTime.UtcNow;
	//    user.ResetToken = null;
	//    user.ResetTokenExpires = null;

	//    await _dataLayerAccountService.UpdateUser(user);
	//}

	//public async Task<List<AccountResponse>> GetAll()
	//{
	//    var accounts = await _dataLayerAccountService.GetAll();
	//    return _mapper.Map<IList<AccountResponse>>(accounts);
	//}

	//public async Task<AccountResponse> GetById(int id)
	//{
	//    var user = getAccount(id);
	//    return _mapper.Map<AccountResponse>(user);
	//}

	//public async Task<AccountResponse> Create(CreateRequest model)
	//{
	//    // validate
	//    var user = _dataLayerAccountService.FindUser(model.Email, model.Email);
	//    if (user != null)
	//        throw new AppException($"Email '{model.Email}' is already registered");

	//    // map model to new user object
	//    var account = _mapper.Map<ApplicationUser>(model);
	//    account.CreatedOn = DateTime.UtcNow;
	//    account.Verified = DateTime.UtcNow;

	//    // hash password
	//    account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

	//    await _dataLayerAccountService.UpdateUser(account);

	//    return _mapper.Map<AccountResponse>(account);
	//}

	//public async Task<AccountResponse> Update(string id, UpdateRequest model)
	//{
	//    var user = await getAccount(id);

	//    // validate
	//    if (user.Email != model.Email && context.User.Any(x => x.Email == model.Email))
	//        throw new AppException($"Email '{model.Email}' is already registered");

	//    // hash password if it was entered
	//    if (!string.IsNullOrEmpty(model.Password))
	//        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

	//    // copy model to user and save
	//    _mapper.Map(model, user);
	//    user.ModifiedOn = DateTime.UtcNow;
	//    context.User.Update(user);
	//    context.SaveChanges();

	//    return _mapper.Map<AccountResponse>(user);
	//}

	public async Task Delete(string id)
	{
		var user = await getAccount(id);
		await _dataLayerAccountService.UpdateUser(user);
	}

	// helper methods

	private async Task<ApplicationUser> getAccount(string id)
	{
		var user = await _dataLayerAccountService.FindUserById(id);

		if (user == null) throw new KeyNotFoundException("Account not found");
		return user;
	}

	private async Task<ApplicationUser> getAccountByRefreshToken(string token)
	{
		var user = await _dataLayerAccountService.GetUserWithRefreshToekn(token);
		if (user == null) throw new AppException("Invalid token");
		return user;
	}

	private async Task<ApplicationUser> getAccountByResetToken(string token)
	{
		var account = await _dataLayerAccountService.GetUserbyResetToekn(token);
		if (account == null) throw new AppException("Invalid token");
		return account;
	}

	private string generateJwtToken(ApplicationUser user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes((string)SettingsConfigHelper.AppSetting("TokenConfig", "Secret"));
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
			Expires = DateTime.UtcNow.AddDays(15),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private async Task<string> generateResetToken()
	{
		// token is a cryptographically strong random sequence of values
		var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

		// ensure token is unique by checking against db
		var tokenIsUnique = !await _dataLayerAccountService.IsTokenAvaiable(token);
		if (!tokenIsUnique)
			return await Task.Run(() => generateResetToken());

		return await Task.Run(() => token);
	}

	private async Task<string> generateVerificationToken()
	{
		// token is a cryptographically strong random sequence of values
		var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

		// ensure token is unique by checking against db
		var tokenIsUnique = !await _dataLayerAccountService.IsVerificationTokenAvailable(token);
		if (!tokenIsUnique)
			return await Task.Run(() => generateVerificationToken());

		return await Task.Run(() => token);
	}

	private async Task<RefreshToken> rotateRefreshToken(RefreshToken refreshToken, string userId, string companyId)
	{
		var newRefreshToken = await _jwtUtils.GenerateRefreshToken(userId, companyId);
		revokeRefreshToken(refreshToken, userId, "Replaced by new token", newRefreshToken.Token);
		return newRefreshToken;
	}

	private void removeOldRefreshTokens(ApplicationUser user)
	{
		var tokenlist = user.RefreshTokens.Where(x =>
			!x.IsActive &&
			x.Created.AddDays(int.Parse(SettingsConfigHelper.AppSetting("TokenConfig", "Secret"))) <= DateTime.UtcNow).ToList();

		user.RefreshTokens.RemoveAll(x =>
			!x.IsActive &&
			x.Created.AddDays(int.Parse(SettingsConfigHelper.AppSetting("TokenConfig", "Secret"))) <= DateTime.UtcNow);
	}

	private void revokeDescendantRefreshTokens(RefreshToken refreshToken, ApplicationUser user, string ipAddress, string reason)
	{
		// recursively traverse the refresh token chain and ensure all descendants are revoked
		if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
		{
			var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
			if (childToken.IsActive)
				revokeRefreshToken(childToken, ipAddress, reason);
			else
				revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
		}
	}

	private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
	{
		token.Revoked = DateTime.UtcNow;
		token.RevokedByIp = ipAddress;
		token.ReasonRevoked = reason;
		token.ReplacedByToken = replacedByToken;
	}

	private void sendVerificationEmail(ApplicationUser user, string origin)
	{
		string message;
		if (!string.IsNullOrEmpty(origin))
		{
			// origin exists if request sent from browser single page app (e.g. Angular or React)
			// so send link to verify via single page app
			var verifyUrl = $"{origin}/user/verify-email?token={user.VerificationToken}";
			message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
		}
		else
		{
			// origin missing if request sent directly to api (e.g. from Postman)
			// so send instructions to verify directly with api
			message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                            <p><code>{user.VerificationToken}</code></p>";
		}

		_emailService.Send(
			to: user.Email,
			subject: "Sign-up Verification API - Verify Email",
			html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
		);
	}

	private void sendAlreadyRegisteredEmail(string email, string origin)
	{
		string message;
		if (!string.IsNullOrEmpty(origin))
			message = $@"<p>If you don't know your password please visit the <a href=""{origin}/user/forgot-password"">forgot password</a> page.</p>";
		else
			message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

		_emailService.Send(
			to: email,
			subject: "Scope - Email Already Registered",
			html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
		);
	}

	private void sendPasswordResetEmail(ApplicationUser user, string origin)
	{
		string message;
		if (!string.IsNullOrEmpty(origin))
		{
			var resetUrl = $"{origin}/user/reset-password?token={user.ResetToken}";
			message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
		}
		else
		{
			message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                            <p><code>{user.ResetToken}</code></p>";
		}

		_emailService.Send(
			to: user.Email,
			subject: "Scope - Reset Password",
			html: $@"<h4>Reset Password Email</h4>
                        {message}"
		);
	}

}