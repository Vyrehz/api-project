using System;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductService
    {
        Products GetAllProducts();

        Products GetProductsByName(string name);

        Product GetProductById(Guid id);

        void SaveProduct(Product product);

        void DeleteProduct(Guid id);
    }
}