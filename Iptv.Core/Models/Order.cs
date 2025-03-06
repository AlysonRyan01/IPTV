using Iptv.Core.Enums;

namespace Iptv.Core.Models;

public class Order
{
    public long Id { get; set; }
    
    public long UserId { get; set; }

    public int TvBoxId { get; set; }
    public TvBox TvBox { get; set; } = new ();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public EPaymentGateway PaymentGateway { get; set; } = EPaymentGateway.Stripe;

    public EOrderStatus OrderStatus { get; set; } = EOrderStatus.AwaitingPayment;

    public decimal ShippingCost { get; set; } = 0;
    
    
}