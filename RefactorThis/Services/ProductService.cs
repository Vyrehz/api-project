using System;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductService : IProductService
    {
        public Products GetAllProducts()
        {
            return new Products();
        }

        public Products GetProductsByName(string name)
        {
            return new Products(name);
        }

        public Product GetProductById(Guid id)
        {
            return new Product(id);
        }

        public void SaveProduct(Product product)
        {
            product.Save();
        }

        public void DeleteProduct(Guid id)
        {
            var product = new Product(id);

            product.Delete();
        }
    }
}