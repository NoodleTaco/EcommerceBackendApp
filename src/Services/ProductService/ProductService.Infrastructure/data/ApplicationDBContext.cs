using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.models;

namespace ProductService.Infrastructure.data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
        : base (dbContextOptions)
        {
            
        }

        public DbSet<ProductModel> Product {get; set;}
    }
}