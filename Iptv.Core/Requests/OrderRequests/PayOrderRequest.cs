namespace Iptv.Core.Requests.OrderRequests;

public class PayOrderRequest
{
    public long UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
}