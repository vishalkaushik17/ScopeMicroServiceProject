using AutoMapper;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.DefaultSettings;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.RequestNResponse.Accounts;

namespace BSAuthentication.BaseClass;

public abstract class BaseBusinessLayer
{
	/// <summary>
	/// Used to check record which is editable or not
	/// </summary>

	public bool UseCache { get; set; }
	internal readonly IMapper _mapper;

	internal ICacheContract _cacheData;
	internal readonly ITrace _trace;

	internal readonly bool _IsTracingRequired;
	internal readonly IHttpContextAccessor _httpContextAccessor;
	protected internal readonly string _UserId;
	protected internal readonly string _UserAccount;
	protected internal readonly string _clientId;
	protected internal readonly string _sessionId;
	protected readonly DateTimeOffset expirationTime;
	//protected MailConfiguration MailConfiguration;

	protected int _modificationInDays = 0;
	protected int _resultPerPage = 0;
	protected ApplicationSettings _applicationSettings;
	protected internal DateTimeOffset _expirationTime;
	public BaseBusinessLayer(IMapper mapper, IHttpContextAccessor httpContextAccessor, ITrace trace)
	{
		_mapper = mapper;
		_httpContextAccessor = httpContextAccessor;
		_trace = trace;
		_IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");

		try
		{
			_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
			_expirationTime = _applicationSettings.ModuleCacheSettings.AuthenticationModule.GetKeyLifeForCacheStorage();

			_UserId = UserIdentity.GetUserId(httpContextAccessor.HttpContext);
			_clientId = UserIdentity.GetClientId(httpContextAccessor.HttpContext);
			_sessionId = UserIdentity.GetSessionId(httpContextAccessor.HttpContext);

			if (_UserId != "Default" && _UserId != null)
			{
				_modificationInDays = httpContextAccessor?.HttpContext?.Items?.Count > 0 ? GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ModificationAllowsInDays) : 0;
				_resultPerPage = httpContextAccessor?.HttpContext?.Items?.Count > 0 ? GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ResultPerPage) : 0;
			}


		}
		catch (Exception ex)
		{
			ex.SendExceptionMailAsync().GetAwaiter().GetResult();
		}
	}

	public T? GetSystemPrefValue<T>(string modelName, string prefName)
	{
		var preferences = _httpContextAccessor?.HttpContext?.Items[$"SystemPreferences-{_clientId}"] as List<SystemPreferenceForSession>;
		var record = preferences?.FirstOrDefault(m => m.ModuleName.Equals(modelName, StringComparison.CurrentCultureIgnoreCase) && m.PreferenceName.Equals(prefName, StringComparison.CurrentCultureIgnoreCase));
		return (T)Convert.ChangeType(record?.Value, typeof(T))!;
	}


	/// <summary>
	/// Generic method for Updating cache records in cache server using
	/// </summary>
	/// <typeparam name="T">Class Name</typeparam>
	/// <param name="modelRecord">model record</param>
	/// <returns>true/false and status as Success/Failed</returns>
	protected async Task<(bool status, string message)> UpdateCahce<T, T2>(T modelRecord, List<T2>? modelRecords)
	{
		try
		{
			//var modelRecords = await _libraryHall.GetAll();
			if (UseCache && modelRecords != null && modelRecords.Count() > 0)
				return await _cacheData.AddCacheAsync(
					_cacheData.GenerateKeyForCache<T>("Index", _clientId), modelRecords,
					_expirationTime);

			return await Task.Run(() => (true, Status.Success));
		}
		catch (Exception e)
		{
			await e.SendExceptionMailAsync();
		}
		return await Task.Run(() => (false, Status.Failed));

	}

}
