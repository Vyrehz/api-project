using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Repositories;

namespace refactor_this.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await _productRepository.GetByNameAsync(name);
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task SaveProductAsync(Product product)
        {
            if (product.IsNew)
            {
                await _productRepository.AddAsync(product);
            }
            else
            {
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task UpsertProductAsync(Guid id, Product updatedProduct)
        {
            var product = await GetProductByIdAsync(id);

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.DeliveryPrice = updatedProduct.DeliveryPrice;

            await SaveProductAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var opt = await _productRepository.GetByIdAsync(id);

            if (opt == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            await _productRepository.DeleteAsync(id);
        }
    }
}