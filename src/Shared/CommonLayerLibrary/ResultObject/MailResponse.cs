using GenericFunction.ServiceObjects.EmailService;
using System.Net;

namespace GenericFunction.ResultObject;

[Serializable]
public class MailResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public string? InternalMessage { get; set; }
    public string FromEmail { get; set; }
    public ServerConfigurationResponse ServerConfigurationResponse { get; set; } = new ServerConfigurationResponse();
}