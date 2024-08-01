using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Core.util;

namespace OrderService.Core.models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        
        public List<OrderItemModel> Items { get; set; }
    }
}