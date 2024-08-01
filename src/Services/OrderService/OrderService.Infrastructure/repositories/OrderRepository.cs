using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Core.interfaces;
using OrderService.Core.models;
using OrderService.Infrastructure.data;

namespace OrderService.Infrastructure.repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }


        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderModel> DeleteOrderAsync(int id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            if(order == null){
                return null;
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrderAsync(int page, int pageSize)
        {
            return await _context.Order
                .Include(o => o.Items)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            return await _context.Order
                .Include(o => o.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Order
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<OrderModel> UpdateOrderAsync(OrderModel order)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == order.Id);

            if(existingOrder == null){
                return null;
            }

            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Status = order.Status;
            
            await _context.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<OrderItemModel> AddOrderItemAsync(int orderId, OrderItemModel item)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == orderId);

            if(existingOrder == null){
                return null;
            }

            item.OrderModelId = orderId;

            _context.OrderItemModel.Add(item);

            await _context.SaveChangesAsync();

            return item;
        }


        public async Task<OrderItemModel> UpdateOrderItemAsync(int itemId, OrderItemModel item)
        {
            var existingOrderItem = await _context.OrderItemModel.FirstOrDefaultAsync(x => x.Id == itemId);

            if(existingOrderItem == null){
                return null;
            }

            existingOrderItem.ProductId = item.ProductId;
            existingOrderItem.Quantity = item.Quantity;
            existingOrderItem.Price = item.Price;

            await _context.SaveChangesAsync();

            return existingOrderItem;
        }

        public async Task<OrderItemModel> RemoveOrderItemAsync(int orderId, int itemId)
        {
            var item = await _context.OrderItemModel
                .FirstOrDefaultAsync(i => i.OrderModelId == orderId && i.Id == itemId);

            if (item == null){
                return null;
            }

            _context.OrderItemModel.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task<OrderItemModel> GetOrderItemByIdAsync(int itemId)
        {
            return _context.OrderItemModel.FirstOrDefaultAsync(x => x.Id == itemId);
        }
    }
}