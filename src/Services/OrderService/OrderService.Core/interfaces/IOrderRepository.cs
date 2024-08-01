using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Core.models;

namespace OrderService.Core.interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderModel>> GetAllOrderAsync(int page, int pageSize);
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(string userId);
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<OrderModel> UpdateOrderAsync(OrderModel order);
        Task<OrderModel> DeleteOrderAsync(int id);
        Task<OrderItemModel> AddOrderItemAsync(int orderId, OrderItemModel item);
        Task<OrderItemModel> GetOrderItemByIdAsync(int itemId);
        Task<OrderItemModel> UpdateOrderItemAsync(int itemId, OrderItemModel item);
        Task<OrderItemModel> RemoveOrderItemAsync(int orderId, int itemId);
    }
}