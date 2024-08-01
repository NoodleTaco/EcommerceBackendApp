using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.API.dtos.OrderDtos;
using OrderService.API.dtos.OrderItemDtos;
using OrderService.Core.models;
using OrderService.Core.util;

namespace OrderService.API.mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToOrderDto(this OrderModel orderModel){
            return new OrderDto{
                Id = orderModel.Id,
                UserId = orderModel.UserId,
                OrderDate = orderModel.OrderDate,
                TotalAmount = orderModel.TotalAmount,
                Status = orderModel.Status,
                Items = orderModel.Items?.Select(o => o.ToOrderItemDto()).ToList() ?? new List<OrderItemDto>()
            };
        }

        public static OrderModel ToOrderFromCreateDto(this CreateOrderDto orderDto){
            if (orderDto == null) return null;

            return new OrderModel
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Items = new List<OrderItemModel>()
            };
        }
    }
}