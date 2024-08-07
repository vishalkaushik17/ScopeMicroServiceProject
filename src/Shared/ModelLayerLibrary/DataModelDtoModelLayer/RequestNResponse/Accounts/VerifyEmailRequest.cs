using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.RequestNResponse.Accounts;

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; }
}