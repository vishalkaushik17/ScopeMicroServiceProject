using GenericFunction.Enums;
using Microsoft.AspNetCore.Http;

namespace GenericFunction.ServiceObjects.EmailService;

public class MailRequest
{
    //[EmailAddress]
    public string?[] ToEmail { get; set; }

    //[EmailAddress]
    public string[]? CCEmail { get; set; }
    public EmailModelSettings? Credential { get; set; }
    public string? Subject { get; set; }
    public EmailNotificationType EmailType { get; set; }
    public string Body { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? UserName { get; set; }
    public string Message { get; set; }

}
