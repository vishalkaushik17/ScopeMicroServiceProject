using Microsoft.AspNetCore.Identity;

namespace ModelTemplates.EntityModels.Application;

public class ApplicationUserAtClient : IdentityUser
{
    public string ClientId { get; set; }
    public bool IsActive { get; set; }
    public DateTime RecordDate { get; set; }
}