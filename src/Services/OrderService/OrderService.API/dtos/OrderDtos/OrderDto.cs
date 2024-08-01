using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.API.dtos.OrderItemDtos;
using OrderService.Core.util;

namespace OrderService.API.dtos.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        
        public List<OrderItemDto> Items { get; set; }
    }
}