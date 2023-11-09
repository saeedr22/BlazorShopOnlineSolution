﻿using ShopOnline.Models;
using ShopOnline.Web.Services.Contract;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Product");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return Enumerable.Empty<ProductDto>();
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }

                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return default(ProductDto);

                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }

                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
