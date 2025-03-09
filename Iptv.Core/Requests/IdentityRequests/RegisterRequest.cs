using System.ComponentModel.DataAnnotations;

namespace Iptv.Core.Requests.IdentityRequests;

public class RegisterRequest
{
    [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O primeiro nome deve ter no máximo 100 caracteres.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "O sobrenome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
    [StringLength(255, ErrorMessage = "O e-mail deve ter no máximo 255 caracteres.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres e no máximo 100.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
    [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não coincidem.")]
    public string ConfirmPassword { get; set; } = null!;
}