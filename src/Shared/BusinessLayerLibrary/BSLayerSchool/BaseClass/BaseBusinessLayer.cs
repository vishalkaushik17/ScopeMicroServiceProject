using AutoMapper;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.Constants.Keys;
using GenericFunction.DefaultSettings;
using GenericFunction.Helpers;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using ModelTemplates.Persistence.Models.AppLevel;
using ModelTemplates.RequestNResponse.Accounts;
using PagedList;
using static GenericFunction.CommonMessages;
namespace BSLayerSchool.BaseClass
{
    public abstract class BaseBusinessLayer
    {
        /// <summary>
        /// Used to check record which is editable or not
        /// </summary>

        public bool UseCache { get; set; }

        protected readonly IMapper _mapper;

        protected ApplicationSettings _applicationSettings;
        protected ICacheContract _cacheData;
        protected readonly ITrace _trace;

        protected readonly bool _IsTracingRequired;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected internal readonly string _userId;
        protected internal readonly string _userAccount;
        protected internal readonly string _clientId;
        protected internal readonly string _sessionId;
        protected MailConfiguration MailConfiguration;

        protected int _modificationInDays = 0;
        protected int _resultPerPage = 0;

        protected int pageNo = 0;
        protected int pageSize = 0;
        protected internal DateTimeOffset _expirationTime;

        protected System.Diagnostics.Stopwatch watch;
        public BaseBusinessLayer(IMapper mapper, IHttpContextAccessor httpContextAccessor, ITrace trace, ICacheContract cache)
        {

            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cacheData = cache;
            _trace = trace;
            _IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
            //watch = System.Diagnostics.Stopwatch.StartNew();
            _applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
            try
            {
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

        /// <summary>
        /// Get system preference value 
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="modelName">Name of model on this system preference get applied.</param>
        /// <param name="prefName">Preference Name</param>
        /// <returns>Return value as per type of T</returns>
        public T? GetSystemPrefValue<T>(string modelName, string prefName)
        {
            var preferences = _httpContextAccessor?.HttpContext?.GetContextItemAsJson<List<SystemPreferenceForSession>>(ContextKeys.SystemPreferences);
            if (preferences == null)
            {
                preferences = _cacheData.ReadFromCacheAsync<SystemPreferenceForSession>(string.Format(CacheKeys.SystemPreferences, UserIdentity.GetClientId(_httpContextAccessor.HttpContext)), 7, true).Result;
            }

            var record = preferences?.FirstOrDefault(m => m.ModuleName.Equals(modelName, StringComparison.CurrentCultureIgnoreCase) && m.PreferenceName.Equals(prefName, StringComparison.CurrentCultureIgnoreCase));
            return (T)Convert.ChangeType(record?.Value, typeof(T))!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelRecord"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>

        //remove it
        protected async Task<(bool status, string message)> UpdateCacheEntry<T>(dynamic modelRecord, bool isNew)
        {
            modelRecord = (T)modelRecord;
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Updating Cache entry", "Process started for adding new record in cache!".MarkInformation());
            var operationStatus = await _cacheData.UpdateCacheAsync<T>("Index", modelRecord.Id, modelRecord, _expirationTime, isNew: isNew);
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Updating Cache entry", "Process ended for adding new record in cache!".MarkInformation());
            return operationStatus;
        }

        protected void DeleteCacheEntry<T>(string id)
        {
            new Thread(new ThreadStart(() =>
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Updating records in cache server!!!");
                _cacheData.RemoveFromCacheAsync<T>("Index", id, _modificationInDays, _expirationTime);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Record updated in cache server!!!");
            })).Start();
        }

        /// <summary>
        /// Send failure response
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="returnMessage">message to send on ui</param>
        /// <returns>ResponseDto</returns>
        protected ResponseDto<T> FailureResponse<T>(string returnMessage) where T : class
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record matched with id!!!");
            ResponseDto<T> dtoModel = new(_httpContextAccessor)
            {
                Message = returnMessage,
                Log = _trace.GetTraceLogs("")
            };
            return dtoModel;
        }

        /// <summary>
        /// Send Success response.
        /// </summary>
        /// <typeparam name="T1">Model object</typeparam>
        /// <typeparam name="T2">Dto object</typeparam>
        /// <param name="id">record Id</param>
        /// <param name="modelRecord">model record</param>
        /// <returns>Success response dto object</returns>
        protected ResponseDto<T2> SuccessResponse<T1, T2>(string id, T1? modelRecord) where T1 : class where T2 : class
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. All success!!!");
            return new(_httpContextAccessor)
            {
                Id = id,
                Log = _trace.GetTraceLogs(""),
                DateTime = DateTime.Now,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<T2>(modelRecord)
            };
        }


        /// <summary>
        /// Send Success response for list object.
        /// </summary>
        /// <typeparam name="T1">Model object</typeparam>
        /// <typeparam name="T2">Dto object</typeparam>
        /// <param name="modelRecord">model record</param>
        /// <returns>Success response dto object</returns>
        protected ResponseDto<T2> SuccessResponse<T1, T2>(List<T1> modelRecords) where T1 : class where T2 : class
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. All success!!!");


            pageSize = pageSize == 0 ? _resultPerPage : pageSize;

            IPagedList<T1> fetchRecords =
               (modelRecords).ToPagedList(pageNo == 0 ? 1 : pageNo, pageSize == 0 ? _resultPerPage : pageSize);

            return new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                DateTime = DateTime.Now,
                TimeConsumption = 0,
                RecordCount = fetchRecords?.Count(),
                CurrentPageNo = fetchRecords != null ? fetchRecords.PageNumber : 0, // pageNo == 0 ? 1 : pageNo,
                Pages = fetchRecords != null ? fetchRecords.PageCount : 0, //(modelRecords.Count + pageSize - 1) / pageSize,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                StatusCode = StatusCodes.Status200OK,
                Result = _mapper.Map<T2>(modelRecords)
            };
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
        //public string GetTraceMethod => TraceMessage();
        public string TraceMessage(
                [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
                [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            //System.Diagnostics.Trace.WriteLine("message: " + message);
            return ($"method name: {memberName} source file : {sourceFilePath} line no: {sourceLineNumber.ToString()}");


        }


        /// <summary>
        /// check duplicate entry in table with fieldname, where fieldname is dyanmic
        /// </summary>
        /// <param name="dto">dto model</param>
        /// <param name="responseDto"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>

        protected ResponseDto<T> DuplicateEntryResponse<T>(string fieldName, string fieldValue) where T : class
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Duplicate record found!!!");
            ResponseDto<T> dtoModel = new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                DateTime = DateTime.Now,
                Message = $"{DuplicateRecordFound} : <br><strong>{fieldName}</strong>: {fieldValue}",
                Status = Status.Failed,
                StatusCode = StatusCodes.Status208AlreadyReported,
            };
            return dtoModel;
        }


    }
}
