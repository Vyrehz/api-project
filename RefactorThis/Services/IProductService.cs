using System;
using System.Collections.Generic;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProductsByName(string name);

        Product GetProductById(Guid id);

        void SaveProduct(Product product);

        void UpsertProduct(Guid id, Product product);

        void DeleteProduct(Guid id);
    }
}