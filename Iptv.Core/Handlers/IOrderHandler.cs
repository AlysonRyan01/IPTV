using Azure;
using Iptv.Core.Models;
using Iptv.Core.Requests.OrderRequests;
using Iptv.Core.Responses;

namespace Iptv.Core.Handlers;

public interface IOrderHandler
{
    Task<BaseResponse<Order?>> CancelAsync(CancelOrderRequest request);
    Task<BaseResponse<Order?>> CreateAsync(CreateOrderRequest request);
    Task<BaseResponse<Order?>> PayAsync(PayOrderRequest request);
    Task<BaseResponse<Order?>> RefundAsync(RefundOrderRequest request);
    Task<BaseResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request);
    Task<BaseResponse<Order?>> GetByNumberAsync(GetOrderByNumberRequest request);
}