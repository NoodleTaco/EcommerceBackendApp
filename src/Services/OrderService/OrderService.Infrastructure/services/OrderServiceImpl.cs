using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Core.interfaces;
using OrderService.Core.models;

namespace OrderService.Infrastructure.services
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderServiceImpl(IOrderRepository orderRepository, IProductService productService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
        }

        public async Task<OrderItemModel> AddOrderItemAsync(int orderId, OrderItemModel item)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order == null){
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            bool productAvailable;
            try
            {
                productAvailable = await _productService.CheckAndUpdateProductQuantityAsync(item.ProductId, item.Quantity);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                throw new Exception("Error while checking product availability", ex);
            }

            if (!productAvailable)
            {
                // Instead of throwing an exception, return null or a custom result
                return null; // Or create a custom result object
            }

            try
            {
                item.Price = await _productService.GetProductPriceAsync(item.ProductId);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                throw new Exception("Error while getting product price", ex);
            }

            var addedItem = await _orderRepository.AddOrderItemAsync(orderId, item);

            order.TotalAmount = CalculateOrderTotal(order.Items);

            await _orderRepository.UpdateOrderAsync(order);

            return addedItem;
        }

        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<OrderModel> DeleteOrderAsync(int id)
        {
            var deletedOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (deletedOrder == null)
            {
                return null;
            }

            // Create a separate list of items to avoid modifying the original collection
            var itemsToProcess = deletedOrder.Items.ToList();

            foreach (var item in itemsToProcess)
            {
                await _productService.CheckAndUpdateProductQuantityAsync(item.ProductId, item.Quantity * -1);
                // We'll remove all items at once after the loop, so we don't need to remove them here
            }

            // Remove all items at once
            foreach (var item in itemsToProcess)
            {
                await _orderRepository.RemoveOrderItemAsync(id, item.Id);
            }

            return await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync(int page, int pageSize)
        {
            return await _orderRepository.GetAllOrderAsync(page, pageSize);
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<OrderItemModel> RemoveOrderItemAsync(int orderId, int itemId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order == null){
                return null;
            }

            var item = await _orderRepository.GetOrderItemByIdAsync(itemId);

            if(item == null){
                return null;
            }

            await _productService.CheckAndUpdateProductQuantityAsync(item.ProductId, item.Quantity * -1);

            var removedItem = await _orderRepository.RemoveOrderItemAsync(orderId, itemId);

            
            order.TotalAmount = CalculateOrderTotal(order.Items);

            await _orderRepository.UpdateOrderAsync(order);

            return removedItem;
        }

        public async Task<OrderModel> UpdateOrderAsync(OrderModel order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }

        

        private decimal CalculateOrderTotal(IEnumerable<OrderItemModel> items)
        {
            return items.Sum(item => item.Price * item.Quantity);
        }


        public async Task<decimal> GetProductPriceAsync(int id){
            return await _productService.GetProductPriceAsync(id);
        }


    }
}