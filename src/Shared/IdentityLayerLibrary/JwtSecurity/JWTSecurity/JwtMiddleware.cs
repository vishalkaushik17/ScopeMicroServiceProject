using DataBaseServices.LayerRepository;
using DataCacheLayer.CacheRepositories.Interfaces;
using DataCacheLayer.CacheRepositories.Repositories;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.RequestNResponse.Accounts;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;
namespace IdentityLayer.JwtSecurity.JWTSecurity;


/// <summary>
/// Default JwtMiddleware for all micro services
/// </summary>
public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly MailConfiguration _mailConfiguration;
	private ICacheContract _cacheData;
	private readonly ApplicationSettings _applicationSettings;
	private DateTimeOffset _expirationTime;
	private List<AppDBHostVsCompanyMaster>? appDBHostVsCompanyMasterRecords = new List<AppDBHostVsCompanyMaster>();
	private readonly bool _isTracingRequired;
	private List<DbConnectionStringRecord> dbHostingRecords;
	private DbConnectionStringRecord? connectedDomainHost;
	private JtwTokenContainerResponse? tokenProperties;
	string clientUserName = string.Empty;
	string sessionId = string.Empty;
	string scopeId = string.Empty;
	public JwtMiddleware(RequestDelegate next, IOptions<MailConfiguration> appSettings, IHttpContextAccessor httpContextAccessor)
	{
		_next = next;
		this._httpContextAccessor = httpContextAccessor;
		_mailConfiguration = appSettings.Value;
		_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
		//this.NameOfClass() = "JwtMiddleware";
		//

		_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");

		dbHostingRecords = new List<DbConnectionStringRecord>();
		connectedDomainHost = new DbConnectionStringRecord();
		tokenProperties = new JtwTokenContainerResponse();
	}


	/// <summary>
	/// On every request this method get invoked. Settings must match with activedemo request code.
	/// </summary>
	/// <param name="context">HttpContext which is used to read header.</param>
	/// <param name="_dbContext">Application db context</param>
	/// <param name="jwtUtils">For Token verification</param>
	/// <param name="httpContextAccessor">HttpContextAccessor which is used to set items to pass next sub sequent request.</param>
	/// <param name="trace">Log tracer</param>
	/// <returns></returns>
	public async Task Invoke(HttpContext context, ApplicationDbContext _dbContext, IJwtUtils jwtUtils, IHttpContextAccessor httpContextAccessor, ITrace trace)
	{
		//initializing cache repository
		_cacheData = new CacheRepositoryService(httpContextAccessor, trace);

		trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), "Process started - Login process start executing!");

		//to store suffix for username eg. cc.in
		string suffixDomain = string.Empty;

		//checking request for Authorization token or Headers
		string? authToken = httpContextAccessor.HttpContext.GetHeader(ContextKeys.Authorization);
		authToken = authToken.Split(" ").Last();
		string?[] suffixDomainArray = new string[] { };

		#region Checking request type
		//if user has passed jwtToken via fetch api.
		if (!string.IsNullOrWhiteSpace(authToken))
		{
			//when request comes with authToken
			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "User has passed Auth Token!".MarkInformation());

			//validating token
			this.tokenProperties = await jwtUtils.ValidateJwtToken(authToken, trace);

			//when token is validated
			if (this.tokenProperties != null)
			{
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Adding session information for logged in user!".MarkInformation());

				//when token is pass this must add in context item
				httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.ClientInfo, this.tokenProperties);


				httpContextAccessor.HttpContext.SetHeader(ContextKeys.UserId, tokenProperties.UserId);
				httpContextAccessor.HttpContext.SetHeader(ContextKeys.ClientId, tokenProperties.ClientId);
				httpContextAccessor.HttpContext.SetHeader(ContextKeys.ClientName, tokenProperties.ClientName);
				httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenSessionId, tokenProperties.TokenSessionId);
				httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenScopeId, tokenProperties.TokenScopeId);

				//now spliting schoolname
				suffixDomainArray = this.tokenProperties.UserName.ToString().Split("@");
				if (suffixDomainArray.Length > 0 && !string.IsNullOrWhiteSpace(suffixDomainArray[1]))
				{
					suffixDomain = suffixDomainArray[1]; // cc.in
				}
			}
			else
			{
				//when invalid token found.
				await _next(context);
				return;
			}
		}
		else
		{
			//on first login attempt we are getting username eg. admin@cc.in here
			//get below information by headers
			clientUserName = httpContextAccessor.HttpContext.GetHeader(ContextKeys.UserName);
			sessionId = httpContextAccessor.HttpContext.GetHeader(ContextKeys.TokenSessionId);
			scopeId = httpContextAccessor.HttpContext.GetHeader(ContextKeys.TokenScopeId);
			httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.FirstInit, true);

			if (!string.IsNullOrWhiteSpace(clientUserName))
			{
				suffixDomainArray = clientUserName.ToString().Split("@");
				if (suffixDomainArray.Length > 0 && !string.IsNullOrWhiteSpace(suffixDomainArray[1]))
				{
					suffixDomain = suffixDomainArray[1];
				}
			}
			else
			{
				//when no credentials found.
				await _next(context);
				return;
			}

		}
		#endregion Checking request type completed.

		#region now setting up env for given request.
		//checking null for suffixDomain eg. cc.in from request given by user or authToken
		if (!string.IsNullOrWhiteSpace(suffixDomain))
		{
			this.appDBHostVsCompanyMasterRecords = await _cacheData.ReadFromCacheAsync<AppDBHostVsCompanyMaster>(string.Format(CacheKeys.AppDBHostVsCompanyMasters, "Common"), 7, true);
			if (this.appDBHostVsCompanyMasterRecords == null || appDBHostVsCompanyMasterRecords.FirstOrDefault(m => m.CompanyMaster.SuffixDomain == suffixDomain) == null)
			{
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Reading for Application Host vs Company Master records from database server!");
				//reading from AppDBHostVsCompanyMaster record for adding records in cache server
				this.appDBHostVsCompanyMasterRecords = await _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster).Include(a => a.ApplicationHostMaster).ToListAsync();

				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), "Reading completed!");
				_expirationTime = _applicationSettings.ModuleCacheSettings.AppDBHostVsCompanyMaster.GetKeyLifeForCacheStorage();
				await _cacheData.AddCacheAsync(string.Format(CacheKeys.AppDBHostVsCompanyMasters, "Common"), appDBHostVsCompanyMasterRecords, _expirationTime);
			}

			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{appDBHostVsCompanyMasterRecords.Count()} - DB Host record found!");

			//getting host information from master database (codingcompanydb);
			//it will call OnConfiguring method inside applicationdbcontext for first time connection.
			trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Valid suffix format!");
			List<DbConnectionStringRecord>? dbConnectionStringRecords = await _cacheData.ReadFromCacheAsync<DbConnectionStringRecord>(string.Format(CacheKeys.DbConnectionStringRecord, "Common"), 7, true);
			if (dbConnectionStringRecords != null && dbConnectionStringRecords.FirstOrDefault(m => m.SuffixName == suffixDomain) != null)
			{
				connectedDomainHost = dbConnectionStringRecords.FirstOrDefault(m => m.SuffixName == suffixDomainArray[1]);
				if (connectedDomainHost != null)
				{
					httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.dbConnectionString, connectedDomainHost);
					httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.DatabaseType, connectedDomainHost.DbType);
					httpContextAccessor.HttpContext.SetHeader(ContextKeys.ClientId, connectedDomainHost.ClientId);
					//httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenSessionId, sessionId);
					//httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenScopeId, scopeId);
					httpContextAccessor.HttpContext.SetHeader(ContextKeys.Suffix, connectedDomainHost.SuffixName);
					httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.IsDemoExipired, connectedDomainHost.IsExpired);
				}
			}
			else
			{
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Hosting records found in cache!");
				foreach (var dbHostCompMasterRecord in appDBHostVsCompanyMasterRecords)
				{
					connectedDomainHost = new DbConnectionStringRecord
					{
						DbConnectionString = dbHostCompMasterRecord.ApplicationHostMaster.ConnectionString,
						SuffixName = dbHostCompMasterRecord.CompanyMaster.SuffixDomain,
						DbType = dbHostCompMasterRecord.ApplicationHostMaster.DatabaseType,
						ClientId = dbHostCompMasterRecord.CompanyMasterId,
						IsExpired = dbHostCompMasterRecord.CompanyMaster.IsDemoExpired
					};

					//add hosting record
					dbHostingRecords.Add(connectedDomainHost);

					//checking domainSuffix is matched with login header suffix value
					if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && dbHostCompMasterRecord.CompanyMaster.SuffixDomain == (suffixDomainArray[1]?.ToLower()))
					{
						trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Adding connection string list in httpContext for suffix{dbHostCompMasterRecord.CompanyMaster.SuffixDomain}!");
						//setting new db connection string in session to access actual database for client 

						//on datacontext access- OnConfiguring method get executed and it will read it from session if its set.
						//and according to that data get pull and sent to UI.
						connectedDomainHost.DbConnectionString = connectedDomainHost.DbConnectionString;
						connectedDomainHost.ClientId = dbHostCompMasterRecord.CompanyMasterId;

						httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.dbConnectionString, connectedDomainHost);
						httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.DatabaseType, dbHostCompMasterRecord.ApplicationHostMaster.DatabaseType);
						//httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.FirstInit, true);

						httpContextAccessor.HttpContext.SetHeader(ContextKeys.Suffix, dbHostCompMasterRecord.CompanyMaster.SuffixDomain);
						httpContextAccessor.HttpContext.SetHeader(ContextKeys.ClientId, dbHostCompMasterRecord.CompanyMasterId);

						httpContextAccessor.HttpContext.SetContextItemAsJson("IsDemoExipired", connectedDomainHost.IsExpired);

						//initializing new database connection as per token properties - domain dbConnectionString.
						trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Initializing dbConnectionString as per login request!");



						if (!string.IsNullOrWhiteSpace(authToken))
						{
							sessionId = tokenProperties.TokenSessionId;
							scopeId = tokenProperties.TokenScopeId;
						}
						httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenSessionId, sessionId);
						httpContextAccessor.HttpContext.SetHeader(ContextKeys.TokenScopeId, scopeId);
					}
				}

				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Adding connection string list in cache!");
				_expirationTime = _applicationSettings.ModuleCacheSettings.DbConnectionStringRecord.GetKeyLifeForCacheStorage();
				await _cacheData.AddCacheAsync<List<DbConnectionStringRecord>>(string.Format(CacheKeys.DbConnectionStringRecord, "Common"), dbHostingRecords, _expirationTime);
			}

			//Get all users from available context db string connection.
			if (!string.IsNullOrWhiteSpace(authToken))
			{
				//var currentUser = await _dbContext.User.FirstOrDefaultAsync(m => m.UserName == tokenProperties.UserName);

				// attach account to context on successful jwt validation
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Setting Account object in context for Custom Authorization!");
				//if (currentUser != null)
				//{
				httpContextAccessor?.HttpContext?.SetContextItemAsJson(ContextKeys.Account, tokenProperties);
				//}
			}

			_dbContext = new ApplicationDbContext(options: _dbContext._Options, httpContextAccessor, trace);
			_httpContextAccessor?.HttpContext?.Items.Add(ContextKeys.ApplicationDbContextObject, _dbContext);

			//httpContextAccessor?.HttpContext?.SetContextItemAsJson(ContextKeys.ApplicationDbContextObject, _dbContext);
			var systemPreferences = await _cacheData.ReadFromCacheAsync<SystemPreferenceForSession>(string.Format(CacheKeys.SystemPreferences, UserIdentity.GetClientId(_httpContextAccessor.HttpContext)), 7, true);

			if (systemPreferences == null)
			{
				//Getting system preference from current company.
				systemPreferences = await _dbContext.SystemPreferences
				.Where(m => m.RecordStatus == GenericFunction.Enums.EnumRecordStatus.Active)
				.Select(m => new SystemPreferenceForSession
				{
					PreferenceName = m.PreferenceName,
					Value = m.CustomValue.ToLower() == "n/a" || m.CustomValue == "0" ? m.DefaultValue : m.CustomValue,
					ModuleName = m.ModuleName,
					ValueType = m.ValueType,

				}).ToListAsync();

				//setting application db context in context item

				httpContextAccessor.HttpContext.SetContextItemAsJson(ContextKeys.SystemPreferences, systemPreferences);
				_expirationTime = _applicationSettings.ModuleCacheSettings.SystemPreferences.GetKeyLifeForCacheStorage();
				await _cacheData.AddCacheAsync(string.Format(CacheKeys.SystemPreferences, UserIdentity.GetClientId(_httpContextAccessor.HttpContext)), systemPreferences, _expirationTime);


			}


			UserIdentity.SetContextItemAsJson(_httpContextAccessor?.HttpContext, ContextKeys.SystemPreferences, systemPreferences);
		}

		#endregion now setting up env for given request end!
		trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), "Process End - Login process executed!");

		await _next(context);
	}
}