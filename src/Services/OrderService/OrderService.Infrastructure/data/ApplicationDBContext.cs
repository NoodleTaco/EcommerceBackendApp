using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Core.models;

namespace OrderService.Infrastructure.data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
        : base (dbContextOptions)
        {
            
        }

        public DbSet<OrderModel> Order {get; set;}

        public DbSet<OrderItemModel> OrderItemModel { get; set; }
    }
}