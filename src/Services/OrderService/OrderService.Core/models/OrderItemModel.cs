using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Core.models
{
    public class OrderItemModel
    {
        public int Id { get; set; }

        public int OrderModelId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
    }
}