using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.API.dtos.OrderItemDtos;
using OrderService.Core.models;

namespace OrderService.API.mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDto ToOrderItemDto(this OrderItemModel orderItemModel){
            return new OrderItemDto{
                Id = orderItemModel.Id,
                OrderModelId = orderItemModel.OrderModelId,
                ProductId = orderItemModel.ProductId,
                Quantity = orderItemModel.Quantity,
                Price = orderItemModel.Price
            };
        }
    }
}