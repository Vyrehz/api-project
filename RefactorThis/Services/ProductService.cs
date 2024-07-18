using System;
using System.Collections.Generic;
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

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _productRepository.GetByName(name);
        }

        public Product GetProductById(Guid id)
        {
            return _productRepository.GetById(id);
        }

        public void SaveProduct(Product product)
        {
            if (product.IsNew)
            {
                _productRepository.Add(product);
            }
            else
            {
                _productRepository.Update(product);
            }
        }

        public void UpsertProduct(Guid id, Product updatedProduct)
        {
            var product = GetProductById(id);

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.DeliveryPrice = updatedProduct.DeliveryPrice;

            SaveProduct(product);
        }

        public void DeleteProduct(Guid id)
        {
            var opt = _productRepository.GetById(id);

            if (opt == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _productRepository.Delete(id);
        }
    }
}