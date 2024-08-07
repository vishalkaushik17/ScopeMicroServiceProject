using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.RequestNResponse.Accounts;

public class ValidateResetTokenRequest
{
    [Required]
    public string Token { get; set; }
}