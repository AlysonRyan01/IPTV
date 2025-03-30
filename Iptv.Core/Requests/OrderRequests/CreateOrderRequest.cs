using System.ComponentModel.DataAnnotations;
using Iptv.Core.Models;

namespace Iptv.Core.Requests.OrderRequests;

public class CreateOrderRequest
{
    public long UserId { get; set; }
    
    [Required(ErrorMessage = "O campo ProductId é obrigatório.")]
    public long ProductId { get; set; }
    
    [Required(ErrorMessage = "O campo frete é obrigatório.")]
    public decimal ShippingCost { get; set; }
    
    [Required(ErrorMessage = "O campo quantidade é obrigatório.")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "O campo Address é obrigatório.")]
    public Address Address { get; set; } = null!;
}