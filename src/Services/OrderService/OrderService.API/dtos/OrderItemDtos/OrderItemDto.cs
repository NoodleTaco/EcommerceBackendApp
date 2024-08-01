using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.dtos.OrderItemDtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        public int OrderModelId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
    }
}