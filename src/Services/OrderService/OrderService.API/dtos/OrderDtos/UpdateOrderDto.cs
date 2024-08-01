using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Core.util;

namespace OrderService.API.dtos.OrderDtos
{
    public class UpdateOrderDto
    {
        public OrderStatus Status { get; set; }
    }
}