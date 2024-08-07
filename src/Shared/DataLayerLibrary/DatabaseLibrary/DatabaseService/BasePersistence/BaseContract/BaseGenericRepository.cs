using DataCacheLayer.CacheRepositories.Interfaces;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.RequestNResponse.Accounts;

namespace DataBaseServices.Persistence.BaseContract;
public interface IDefaultInterfaceServices
{
	Task<ApplicationDbContext> GetDbContextAsyncAsync();
	Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext);
}

public abstract class BaseGenericRepository<T> where T : class //: IGenericContract<T, T2> where T : class where T2 : class
{
	/// <summary>
	/// Used to check record which is editable or not
	/// </summary>

	protected ApplicationSettings _applicationSettings;
	public bool UseCache { get; set; }
	public bool IsRecordEditable = false;
	public bool IsRecordFound = false;

	internal ICacheContract _cacheData;
	internal ApplicationDbContext? _dbContext;
	internal readonly ITrace _trace;


	internal readonly IHttpContextAccessor? _httpContextAccessor;
	protected internal readonly string? _userId;
	protected internal readonly string? _clientId;
	protected internal readonly string? _sessionId;

	protected MailConfiguration? MailConfiguration;

	protected int _modificationInDays = 0;
	protected int _resultPerPage = 0;
	protected readonly bool _isTracingRequired;
	protected internal DateTimeOffset _expirationTime;
	//internal DbSet<T> _dbSet;
	public BaseGenericRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace)
	{

		try
		{
			var isAppDbContextAvailable = httpContextAccessor?.HttpContext?.Items[ContextKeys.ApplicationDbContextObject];
			if (isAppDbContextAvailable != null)
			{
				_dbContext = isAppDbContextAvailable as ApplicationDbContext;
			}
			else
			{
				_dbContext = dbContext;
			}

		}
		catch (Exception ex)
		{

			throw;
		}

		_trace = trace;
		_httpContextAccessor = httpContextAccessor;




		_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
		_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();

		try
		{
			_userId = UserIdentity.GetUserId(httpContextAccessor.HttpContext);
			_clientId = UserIdentity.GetClientId(httpContextAccessor.HttpContext);
			_sessionId = UserIdentity.GetSessionId(httpContextAccessor.HttpContext);

			if (_userId != "Default" && _userId != null)
			{
				_modificationInDays = httpContextAccessor?.HttpContext?.Items?.Count > 0 ? GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ModificationAllowsInDays) : 0;
				_resultPerPage = httpContextAccessor?.HttpContext?.Items?.Count > 0 ? GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ResultPerPage) : 0;
			}
			_trace.AddTrace($"BaseGenericRepository : Calling From : {base.ToString()}", ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"UserId found : {_userId} ClientId : {_clientId} & SessionId :{_sessionId} ".MarkInformation());
		}
		catch (Exception)
		{
			//do nothing
		}

		_trace.AddTrace($"BaseGenericRepository : Calling From : {base.ToString()}", ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Calling Class : {base.ToString()}".MarkInformation());

	}

	/// <summary>
	/// Get System preference value, here at the time of checking session login
	/// in JwtMiddleware, we are assigning value from the database.
	/// Where we are checking if system value is overridden by custom value
	/// then custom value get take over and session is set for custom value.
	/// </summary>
	/// <typeparam name="T">Generic return type.</typeparam>
	/// <param name="modelName">Name of the model where preference get checked.</param>
	/// <param name="prefName">Name of the preference which is going to check</param>
	/// <returns>Returns preference value on basis of generic return type after converting.</returns>
	public T? GetSystemPrefValue<T>(string modelName, string prefName)
	{

		var preferences = _httpContextAccessor?.HttpContext?.GetContextItemAsJson<List<SystemPreferenceForSession>>(ContextKeys.SystemPreferences);
		var record = preferences?.FirstOrDefault(m => m.ModuleName.Equals(modelName, StringComparison.CurrentCultureIgnoreCase) && m.PreferenceName.Equals(prefName, StringComparison.CurrentCultureIgnoreCase));
		_trace.AddTrace($"BaseGenericRepository : Calling From : {base.ToString()}", ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Total <b>{preferences?.Count} Preferences </b> found! Preferences <b>{record?.PreferenceName} </b> found!");

		return (T)Convert.ChangeType(record?.Value, typeof(T))!;
	}

	public virtual async Task<ApplicationDbContext> GetDbContextAsyncAsync()
	{
		return await Task.Run(() => _dbContext);
	}

	public virtual async Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		return await Task.Run(() => _dbContext);
	}

}