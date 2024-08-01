using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OrderService.Core.interfaces;

namespace OrderService.Infrastructure.services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ProductServiceUrl"];
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> CheckAndUpdateProductQuantityAsync(int productId, int quantityChange)
        {
            AddAuthorizationHeader();
            var content = new StringContent(quantityChange.ToString(), Encoding.UTF8, "application/json");

            string contentString = await content.ReadAsStringAsync();
            Console.WriteLine("The content is: " + contentString);

            var response = await _httpClient.PutAsync(
                $"{_baseUrl}/api/product/{productId}/quantity",
                content
            );

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return false; // Product not available in requested quantity
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error updating product quantity: {response.StatusCode}. Response: {responseBody}");
            }
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/product/{productId}/price");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productPrice = JsonSerializer.Deserialize<decimal>(content);
                return productPrice;
            }
            else
            {
                Console.WriteLine(response.ToString());
                throw new HttpRequestException($"Error getting product price: {response.StatusCode}");
            }
        }
    }
}