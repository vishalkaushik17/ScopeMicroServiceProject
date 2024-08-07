using GenericFunction;
using GenericFunction.ResultObject;
using ModelTemplates.RequestNResponse.Accounts;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;

namespace ScopeCoreWebApp.Data
{
	public class WebApiExecutorService : IWebApiExecutor
	{
		private readonly IHttpClientFactory httpClientFactory;
		public HttpClient httpClient { get; set; }
		public const string apiName = "login";

		private readonly bool _IsTracingRequired;
		public WebApiExecutorService(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;

			_IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
		}

		public async Task<T?> InvokeGet<T>(string relativeUrl)
		{
			var httpClient = httpClientFactory.CreateClient(apiName);

			return await httpClient.GetFromJsonAsync<T>(relativeUrl);

		}

		public async Task<ResponseDto<ApplicationUserDtoModel>> InvokePost<T>(string relativeUrl, T loginModal, string username, string TokenSessionId, string TokenScopeId, ITrace trace)
		{

			var baseUrl = AppSettingsConfigurationManager.AppSetting.GetValue<string>("ApiBaseUrl");
			var apiTimeout = AppSettingsConfigurationManager.AppSetting.GetValue<int>("ApiTimeout");
			ResponseDto<ApplicationUserDtoModel> responseDto = new ResponseDto<ApplicationUserDtoModel>(new HttpContextAccessor());
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				// ITrace trace = new TraceRepository(httpContextAccessor);
				trace.AddTrace("WebApiExecutorService", ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "OperationStart.ToCss()", $"API Url : {baseUrl}{relativeUrl}".MarkInformation());
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, " API Response Log : ", "{{replaceApiResponse}}");

				var httpClient = httpClientFactory.CreateClient(apiName);
				httpClient.BaseAddress = new Uri(baseUrl);
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				httpClient.DefaultRequestHeaders.Add("UserName", username);
				httpClient.DefaultRequestHeaders.Add("TokenSessionId", TokenSessionId);
				httpClient.DefaultRequestHeaders.Add("TokenScopeId", TokenScopeId);
				httpClient.Timeout = TimeSpan.FromMinutes(apiTimeout);

				response = await httpClient.PostAsJsonAsync<T>(relativeUrl, loginModal);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					response.EnsureSuccessStatusCode();
					trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"API call finished!".MarkInformation());
					responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<ApplicationUserDtoModel>>();

					//switch (responseDto.StatusCode)
					//{
					//    case 401:

					//    default:
					//        break;
					//}

					trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Response phase message : ".MarkInformation(), responseDto.Message.MarkProcess());
					//responseDto.Message = response.RequestMessage.ToString();
					return responseDto;
				}
				switch (response.StatusCode)
				{
					case System.Net.HttpStatusCode.ServiceUnavailable:
						responseDto.Message = "Api gateway service is down!";
						break;
					default:
						responseDto.Message = "Internal server error!";
						break;
				}
				responseDto.StatusCode = (int)response.StatusCode;
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"API call finished!".MarkInformation());
			}
			catch (Exception ex)
			{
				responseDto.Message = response.ToString();
				responseDto.Exception = ex;
				trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"Error : {ex.Message}".MarkInformation());
			}

			return responseDto;
		}
	}
}
