using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.API.dtos;
using ProductService.Core.models;

namespace ProductService.API.mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this ProductModel productModel){
            return new ProductDto{
                Id = productModel.Id,
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Quantity = productModel.Quantity,
                SellerUsername = productModel.SellerUsername
            };
        }

        public static ProductModel ToProductModelFromCreateDto(this CreateProductDto productDto){
            return new ProductModel{
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                SellerUsername = productDto.SellerUsername
            };
        }

        public static ProductModel ToProductModelFromUpdateDto(this UpdateProductDto productDto){
            return new ProductModel{
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                SellerUsername = productDto.SellerUsername
            };
        }
    }
}