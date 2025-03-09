namespace Iptv.Core.Requests.OrderRequests;

public class CancelOrderRequest
{
    public long UserId { get; set; }
    public long Id { get; set; }
}