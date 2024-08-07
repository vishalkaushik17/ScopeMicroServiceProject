using AutoMapper;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.DefaultSettings;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.Persistence.Models.School.CommonModels;
using ModelTemplates.RequestNResponse.Accounts;
using static GenericFunction.CommonMessages;
namespace BSCodingCompany.BaseClass;

public abstract class BaseBusinessLayer
{
	/// <summary>
	/// Used to check record which is editable or not
	/// </summary>

	public bool UseCache { get; set; }

	internal readonly IMapper _mapper;
	private readonly bool _isTracingRequired;

	protected ApplicationSettings _applicationSettings;
	internal ICacheContract _cacheData;
	internal readonly ITrace _trace;


	internal readonly bool _IsTracingRequired;
	internal readonly IHttpContextAccessor _httpContextAccessor;
	protected internal readonly string _userId;
	protected internal readonly string _userAccount;
	protected internal readonly string _clientId;
	protected internal readonly string _sessionId;
	protected MailConfiguration MailConfiguration;

	protected int _modificationInDays = 0;
	protected int _resultPerPage = 0;

	protected internal DateTimeOffset _expirationTime;

	protected System.Diagnostics.Stopwatch watch;
	public BaseBusinessLayer(IMapper mapper, IHttpContextAccessor httpContextAccessor, ITrace trace, ICacheContract cache)
	{

		_mapper = mapper;
		_httpContextAccessor = httpContextAccessor;
		_cacheData = cache;
		_trace = trace;
		_IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
		// watch = System.Diagnostics.Stopwatch.StartNew();

		_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();


		try
		{
			_expirationTime = _applicationSettings.ModuleCacheSettings.LibraryModule.GetKeyLifeForCacheStorage();
			_userId = UserIdentity.GetUserId(httpContextAccessor.HttpContext);
			_clientId = UserIdentity.GetClientId(httpContextAccessor.HttpContext);
			_sessionId = UserIdentity.GetSessionId(httpContextAccessor.HttpContext);

			if (_userId != "Default" && _userId != null)
			{
				_modificationInDays = GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ModificationAllowsInDays);
				_resultPerPage = GetSystemPrefValue<int>(AppPreferences.All, AppPreferences.ResultPerPage);
			}


		}
		catch (Exception ex)
		{
			ex.SendExceptionMailAsync().GetAwaiter().GetResult();
		}
	}

	public T? GetSystemPrefValue<T>(string modelName, string prefName)
	{
		string prefKey = $"{nameof(SystemPreferencesModel)}-{_clientId}";
		var preferences = _httpContextAccessor?.HttpContext?.GetContextItemAsJson<List<SystemPreferenceForSession>>(prefKey);
		if (preferences == null)
		{
			preferences = _cacheData.ReadFromCacheAsync<SystemPreferenceForSession>(prefKey, 7, true).Result;
		}

		var record = preferences?.FirstOrDefault(m => m.ModuleName.Equals(modelName, StringComparison.CurrentCultureIgnoreCase) && m.PreferenceName.Equals(prefName, StringComparison.CurrentCultureIgnoreCase));
		return (T)Convert.ChangeType(record?.Value, typeof(T))!;
	}

	protected async Task<(bool status, string message)> UpdateCacheEntry<T>(dynamic modelRecord, bool isNew)
	{
		//new Thread(new ThreadStart(() =>
		//  {
		modelRecord = (T)modelRecord;
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Updating Cache entry", "Process started for adding new record in cache!".MarkInformation());
		var temp = await _cacheData.UpdateCacheAsync<T>("Index", modelRecord.Id, modelRecord, _expirationTime, isNew: isNew);
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Updating Cache entry", "Process ended for adding new record in cache!".MarkInformation());
		//})).Start();
		return temp;
	}

	protected void DeleteCacheEntry(string id)
	{
		new Thread(new ThreadStart(() =>
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Updating records in cache server!!!");
			_cacheData.RemoveFromCacheAsync<CurrencyModel>("Index", id, _modificationInDays, _expirationTime);
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Record updated in cache server!!!");
		})).Start();
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
			e.SendExceptionMailAsync().GetAwaiter().GetResult();
			await e.SendExceptionMailAsync();
		}
		return await Task.Run(() => (false, Status.Failed));

	}
	//public string GetTraceMethod => TraceMessage();
	public string TraceMessage(
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
	{
		//System.Diagnostics.Trace.WriteLine("message: " + message);
		return ($"method name: {memberName} source file : {sourceFilePath} line no: {sourceLineNumber.ToString()}");


	}


	//protected object ReturnSuccess<T>(dynamic modelRecord)
	//{
	//    return new 
	//    {
	//        Log = _trace.GetTraceLogs(""),
	//        Id = modelRecord.Id,
	//        DateTime = DateTime.Now,
	//        Message = OperationSuccessful,
	//        MessageType = MessageType.Information,
	//        Status = Status.Success,
	//        RecordCount = 1,
	//        StatusCode = StatusCodes.Status200OK,
	//        TimeConsumption = 0,
	//        Result = _mapper.Map<T>(modelRecord)
	//    };
	//}
	protected void DuplicateEntryCheck(dynamic dto, dynamic responseDto, string fieldName, string fieldValue)
	{
		//watch.Stop();
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record already exists!!!");

		responseDto.Log = _trace.GetTraceLogs("");
		responseDto.TimeConsumption = 0;
		responseDto.Id = dto.Id;
		responseDto.MessageType = MessageType.Warning;
		responseDto.Message = $"{DuplicateRecordFound} <br><strong>{fieldName}</strong> : {fieldValue}";
		responseDto.Result = dto;
		responseDto.Status = Status.Failed;
		responseDto.StatusCode = StatusCodes.Status208AlreadyReported;
	}
}

