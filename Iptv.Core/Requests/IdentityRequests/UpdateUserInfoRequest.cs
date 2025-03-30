using System.ComponentModel.DataAnnotations;
using Iptv.Core.Models;

namespace Iptv.Core.Requests.IdentityRequests;

public class UpdateUserInfoRequest
{
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O primeiro nome deve ter no máximo 50 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O sobrenome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O sobrenome deve ter no máximo 50 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "O telefone informado não é válido.")]
    public string Phone { get; set; } = string.Empty;
}