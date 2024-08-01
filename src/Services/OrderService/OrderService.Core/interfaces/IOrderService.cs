using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Core.models;

namespace OrderService.Core.interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<OrderModel> UpdateOrderAsync(OrderModel order);
        Task<OrderModel> DeleteOrderAsync(int id);
        Task<OrderItemModel> AddOrderItemAsync(int orderId, OrderItemModel item);
        Task<OrderItemModel> RemoveOrderItemAsync(int orderId, int itemId);

        Task<IEnumerable<OrderModel>> GetAllOrdersAsync(int page, int pageSize);
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(string userId);
        
        Task<decimal> GetProductPriceAsync(int id);
    }
}