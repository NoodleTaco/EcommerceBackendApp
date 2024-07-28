using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Core.models;

namespace ProductService.Core.interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync(int page, int pageSize);
        Task<ProductModel> CreateProductAsync(ProductModel productModel);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductModel>> GetProductsBySellerUsernameAsync(string sellerUsername, int page, int pageSize);
        Task<ProductModel> UpdateProductAsync(ProductModel productModel);
        Task<ProductModel> DeleteProductAsync(int id);
    }
}