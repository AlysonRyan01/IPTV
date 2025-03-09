using System.ComponentModel.DataAnnotations;

namespace Iptv.Core.Requests.OrderRequests;

public class GetOrderByNumberRequest
{
    public long UserId { get; set; }
    [Required(ErrorMessage = "O campo Número é obrigatório.")]
    public string Number { get; set; } = string.Empty;
}