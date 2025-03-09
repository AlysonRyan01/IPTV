namespace Iptv.Core.Enums;

public enum EOrderStatus
{
    WaitingPayment = 1,
    Paid = 2,
    Canceled = 3,
    Refunded = 4,
    Preparing = 5,
    Shipping = 6,
    Delivered = 7
}