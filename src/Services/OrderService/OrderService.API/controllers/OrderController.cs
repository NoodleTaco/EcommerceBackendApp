using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.dtos.OrderDtos;
using OrderService.API.dtos.OrderItemDtos;
using OrderService.API.mappers;
using OrderService.Core.interfaces;
using OrderService.Core.models;
using OrderService.Core.util;

namespace OrderService.API.controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var orders = await _orderService.GetAllOrdersAsync(page, pageSize);

            var orderDtos = orders.Select(o => o.ToOrderDto());

            return Ok(orderDtos);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var order = await _orderService.GetOrderByIdAsync(id);
            
            if(order == null){
                return NotFound();
            }

            return Ok(order.ToOrderDto());
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var orderModel = createOrderDto.ToOrderFromCreateDto();

            if (orderModel == null)
            {
                return BadRequest("Failed to create order model from DTO");
            }

            orderModel.OrderDate = DateTime.UtcNow;

            orderModel.Status = OrderStatus.Pending;

            orderModel.TotalAmount = 0;

            orderModel.Items = new List<OrderItemModel>();

            await _orderService.CreateOrderAsync(orderModel);

            return CreatedAtAction(nameof(GetById), new {id = orderModel.Id}, orderModel.ToOrderDto());
        }



        [HttpGet("{username}/user")]
        public async Task<IActionResult> GetUserOrders([FromRoute] string username)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var orders = await _orderService.GetOrdersByUserIdAsync(username);

            var orderDtos = orders.Select(o => o.ToOrderDto());

            return Ok(orderDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto updateOrderDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var orderFound = await _orderService.GetOrderByIdAsync(id);

            if(orderFound == null){
                return NotFound();
            }

            orderFound.Status = updateOrderDto.Status;

            await _orderService.UpdateOrderAsync(orderFound);

            return Ok(orderFound);
        }

        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddOrderItem(int orderId, [FromBody] AddOrderItemDto addOrderItemDto){
            var orderItemModel = new OrderItemModel {
                ProductId = addOrderItemDto.ProductId,
                Quantity = addOrderItemDto.Quantity,
                OrderModelId = orderId
                
            };

            try 
            {
                var addedItem = await _orderService.AddOrderItemAsync(orderId, orderItemModel);
                if (addedItem == null)
                {
                    return BadRequest("Product is not available in the requested quantity.");
                }
                return CreatedAtAction(nameof(GetById), new { id = orderId }, addedItem);
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("{productId}/product")]
        public async Task<IActionResult> GetProductPrice([FromRoute] int productId)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var price = await _orderService.GetProductPriceAsync(productId);

            return Ok(price);
        }

        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoveOrderItem([FromRoute] int orderId, [FromRoute] int itemId){
            var removedItem = await _orderService.RemoveOrderItemAsync(orderId, itemId);
            if(removedItem == null){
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deletedOrder = await _orderService.DeleteOrderAsync(id);
            if(deletedOrder == null){
                return NotFound();
            }
            return NoContent();
        }

        
    }
}