using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.dtos;
using ProductService.API.mappers;
using ProductService.Core.interfaces;
using ProductService.Core.models;
using ProductService.Infrastructure.data;

namespace ProductService.API.controllers
{

    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var products = await _productRepository.GetAllProductsAsync(page, pageSize);

            var productDto = products.Select(p => p.ToProductDto());

            return Ok(productDto);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var product = await _productRepository.GetProductByIdAsync(id);
            
            if(product == null){
                return NotFound();
            }

            return Ok(product.ToProductDto());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto productDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var productModel = productDto.ToProductModelFromCreateDto();

            await _productRepository.CreateProductAsync(productModel);

            return CreatedAtAction(nameof(GetById), new {id = productModel.Id}, productModel.ToProductDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            ProductModel productModel = updateProductDto.ToProductModelFromUpdateDto();

            productModel.Id = id;

            await _productRepository.UpdateProductAsync(productModel);

            return Ok(productModel.ToProductDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var productModel = await _productRepository.DeleteProductAsync(id);

            if(productModel == null){
                return NotFound();
            }

            return NoContent();
        }






    }
}