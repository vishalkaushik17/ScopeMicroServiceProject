using GenericFunction.Constants.Keys;
using GenericFunction.ServiceObjects.EmailService;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using static GenericFunction.ResultObject.Status;
namespace GenericFunction.ResultObject;


[Serializable]
public class ResponseDto<T> where T : class
{
	public ResponseDto()
	{

	}
	public ResponseDto(IHttpContextAccessor httpContextAccessor)
	{
		ClientId = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.ClientId) ?? "Default";
		ClientName = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.ClientName) ?? "Default";
		UserName = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.UserName) ?? "Default";
		UserId = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.UserId) ?? "Default";
	}
	public string ClientId { get; set; } = string.Empty;
	public string ClientName { get; set; } = string.Empty;

	public string? UserId { get; set; }
	public string? UserName { get; set; }
	public string? Password { get; set; }

	public string? Id { get; set; }
	//    public string? Token { get; set; }
	public DateTime? Expiration { get; set; }
	public string? Status { get; set; } = Failed; //need to implement enum ResponseStatus
	public string? Message { get; set; } = CommonMessages.OperationFailed;
	//public object? ReturnObject { get; set; }
	//public Clock? Clock { get; set; } = new Clock();
	public double TimeConsumption { get; set; } = 0;


	public string? Log { get; set; } = string.Empty;
	public int StatusCode { get; set; }//= StatusCodes.Status404NotFound;
	public IList<string> ModelStateErrors { get; set; } = new List<string>();

	public ExceptionOnMethod ExceptionRaisedOn { get; set; } = new ExceptionOnMethod();

	[JsonPropertyName("Result")]

	public T? Result { get; set; } = default(T);


	public int? RecordCount { get; set; } = 0;

	/// <summary>
	/// Total pages
	/// </summary>
	public int Pages { get; set; } = 1;

	/// <summary>
	/// Current Page no
	/// </summary>
	public int CurrentPageNo { get; set; } = 1;

	public ServerConfigurationResponse ServerConfigurationResponse { get; set; }


	public string MessageType { get; set; } = ResultObject.MessageType.Warning;
	public Exception? Exception { get; set; } = null;
	public DateTime DateTime { get; set; } = DateTime.Now;
	public bool EmailStatus { get; set; } = false;
	public string? EmailResponse { get; set; } = string.Empty;

}