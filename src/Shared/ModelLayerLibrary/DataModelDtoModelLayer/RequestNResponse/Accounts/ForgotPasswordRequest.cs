using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.RequestNResponse.Accounts;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}