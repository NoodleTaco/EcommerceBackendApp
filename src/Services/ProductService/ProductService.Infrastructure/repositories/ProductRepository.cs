using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.interfaces;
using ProductService.Core.models;
using ProductService.Infrastructure.data;

namespace ProductService.Infrastructure.repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public async Task<ProductModel> CreateProductAsync(ProductModel productModel)
        {
            _context.Product.Add(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<ProductModel> DeleteProductAsync(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            if(product == null){
                return null;
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync(int page, int pageSize)
        {
            return await _context.Product
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ProductModel>> GetProductsBySellerUsernameAsync(string sellerUsername, int page, int pageSize)
        {
            return await _context.Product
                .Where(p => p.SellerUsername == sellerUsername)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel productModel)
        {
            var existingProduct = await _context.Product.FirstOrDefaultAsync(x => x.Id == productModel.Id);
            if(existingProduct == null){
                return null;
            }

            existingProduct.Name = productModel.Name;
            existingProduct.Description = productModel.Description;
            existingProduct.Price = productModel.Price;
            existingProduct.Quantity = productModel.Quantity;
            existingProduct.SellerUsername = productModel.SellerUsername;

            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}