using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.API.dtos.OrderDtos
{
    public class CreateOrderDto
    {
        [Required]
        public string UserId { get; set; }
    }
}