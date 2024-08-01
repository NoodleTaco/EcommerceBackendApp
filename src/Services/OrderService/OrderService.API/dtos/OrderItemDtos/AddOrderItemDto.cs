using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.dtos.OrderItemDtos
{
    public class AddOrderItemDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}