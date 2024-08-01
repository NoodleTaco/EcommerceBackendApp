using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Core.interfaces
{
    public interface IProductService
    {
        Task<bool> CheckAndUpdateProductQuantityAsync(int productId, int quantityChange);
        Task<decimal> GetProductPriceAsync(int productId);
    }
}