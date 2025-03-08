using Iptv.Core.Enums;

namespace Iptv.Core.Models;

public class Order
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public string Number { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string? ExternalReference { get; set; } = null;

    public long TvBoxId { get; set; }
    public TvBox TvBox { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public EPaymentGateway PaymentGateway { get; set; } = EPaymentGateway.Stripe;

    public EOrderStatus OrderStatus { get; set; } = EOrderStatus.AwaitingPayment;

    public Address Address { get; set; } = null!;
    public long AddressId { get; set; }

    public decimal ShippingCost { get; set; }
    
    public decimal Amount => ((TvBox.Amount * TvBox.Quantity) + (ShippingCost * TvBox.Quantity));
    
    
}