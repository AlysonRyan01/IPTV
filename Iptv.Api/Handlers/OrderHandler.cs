using Iptv.Api.Data;
using Iptv.Core.Enums;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.OrderRequests;
using Iptv.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Iptv.Api.Handlers;

public class OrderHandler(IptvDbContext context) : IOrderHandler
{
    public async Task<BaseResponse<Order?>> CancelAsync(CancelOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context
                .Orders
                .Include(x => x.TvBox)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new BaseResponse<Order?>(null, 404, "Pedido não encontrado");
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Falha ao obter pedido");
        }

        switch (order.OrderStatus)
        {
            case EOrderStatus.Canceled:
                return new BaseResponse<Order?>(order, 400, "O pedido já foi cancelado!");

            case EOrderStatus.Paid:
                return new BaseResponse<Order?>(order, 400, "Um pedido pago não pode ser cancelado!");

            case EOrderStatus.Refunded:
                return new BaseResponse<Order?>(order, 400, "Um pedido reembolsado não pode ser cancelado");

            case EOrderStatus.WaitingPayment:
                break;

            default:
                return new BaseResponse<Order?>(order, 400, "Pedido com situação inválida!");
        }

        order.OrderStatus = EOrderStatus.Canceled;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new BaseResponse<Order?>(order, 500, "Não foi possível atualizar seu pedido");
        }

        return new BaseResponse<Order?>(order, 200, $"Pedido {order.Number} atualizado!");
    }

    public async Task<BaseResponse<Order?>> CreateAsync(CreateOrderRequest request)
    {
        TvBox? product;
        try
        {
            product = await context
                .TvBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (product is null)
                return new BaseResponse<Order?>(null, 404, "Produto não encontrado ou inativo");

            context.Attach(product);
            context.Attach(request.Address);
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Falha ao verificar produto");
        }

        var order = new Order
        {
            UserId = request.UserId,
            TvBox = product,
            TvBoxId = request.ProductId,
            ShippingCost = request.ShippingCost,
            Quantity = request.Quantity,
            Address = request.Address,
            AddressId = request.Address.Id
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Não foi possível registrar seu pedido");
        }

        return new BaseResponse<Order?>(order, 201, $"Pedido {order.Number} cadastrado com sucesso!");
    }

    public async Task<BaseResponse<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context
                .Orders
                .Include(x => x.TvBox)
                .FirstOrDefaultAsync(x => x.Number == request.OrderNumber && x.UserId == request.UserId);

            if (order is null)
                return new BaseResponse<Order?>(null, 404, $"Pedido {request.OrderNumber} não encontrado");
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Falha ao consultar pedido");
        }

        switch (order.OrderStatus)
        {
            case EOrderStatus.Canceled:
                return new BaseResponse<Order?>(order, 400, "O pedido está cancelado!");

            case EOrderStatus.Paid:
                return new BaseResponse<Order?>(order, 400, "O pedido já foi pago");

            case EOrderStatus.Refunded:
                return new BaseResponse<Order?>(order, 400, "Um pedido reembolsado não pode ser cancelado");

            case EOrderStatus.WaitingPayment:
                break;

            default:
                return new BaseResponse<Order?>(order, 400, "Situação do pedido inválida");
        }

        //Codigo stripe

        order.OrderStatus = EOrderStatus.Paid;
        order.ExternalReference = request.ExternalReference;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new BaseResponse<Order?>(order, 500, "Não foi possível realizar o pagamento do seu pedido!");
        }

        return new BaseResponse<Order?>(order, 200, $"Pedido {order.Number} pago com sucesso!");
    }

    public async Task<BaseResponse<Order?>> RefundAsync(RefundOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context
                .Orders
                .Include(x => x.TvBox)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new BaseResponse<Order?>(null, 404, "Pedido não encontrado");
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Falha ao consultar seu pedido");
        }

        switch (order.OrderStatus)
        {
            case EOrderStatus.Canceled:
                return new BaseResponse<Order?>(order, 400, "O pedido está cancelado!");

            case EOrderStatus.WaitingPayment:
                return new BaseResponse<Order?>(order, 400, "O pedido ainda não foi pago");

            case EOrderStatus.Refunded:
                return new BaseResponse<Order?>(order, 400, "O pedido já foi reembolsado");

            case EOrderStatus.Paid:
                break;

            default:
                return new BaseResponse<Order?>(order, 400, "Situação do pedido inválida");
        }

        order.OrderStatus = EOrderStatus.Refunded;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new BaseResponse<Order?>(order, 500, "Não foi possível reembolsar seu pedido");
        }

        return new BaseResponse<Order?>(order, 200, $"Pedido {order.Number} estornado com sucesso!");
    }

    public async Task<BaseResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        try
        {
            var orders = await context
                .Orders
                .AsNoTracking()
                .Include(x => x.TvBox)
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            return new BaseResponse<List<Order>?>(orders, 200, "Pedidos com sucesso!");
        }
        catch
        {
            return new BaseResponse<List<Order>?>(null, 500, "Não foi possível consultar os pedidos");
        }
    }

    public async Task<BaseResponse<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        try
        {
            var order = await context
                .Orders
                .AsNoTracking()
                .Include(x => x.TvBox)
                .FirstOrDefaultAsync(x => x.Number == request.Number && x.UserId == request.UserId);

            return order is null
                ? new BaseResponse<Order?>(null, 404, "Pedido não encontrado")
                : new BaseResponse<Order?>(order);
        }
        catch
        {
            return new BaseResponse<Order?>(null, 500, "Não foi possível recuperar o pedido");
        }
    }
}