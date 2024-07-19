using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);

        Task<Product> GetProductByIdAsync(Guid id);

        Task SaveProductAsync(Product product);

        Task UpsertProductAsync(Guid id, Product product);

        Task DeleteProductAsync(Guid id);
    }
}