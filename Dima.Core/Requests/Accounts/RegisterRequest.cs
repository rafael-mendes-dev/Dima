using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Accounts;

public class RegisterRequest : Request
{
    [Required(ErrorMessage = "Informe um e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Informe uma senha.")]
    public string Password { get; set; } = string.Empty;
}