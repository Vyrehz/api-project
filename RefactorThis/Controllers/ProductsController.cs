using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;

        public ProductsController(IProductService productService, IProductOptionService productOptionService)
        {
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [Route]
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _productService.GetAllProducts();
        }

        [Route]
        [HttpGet]
        public IEnumerable<Product> SearchByName(string name)
        {
            return _productService.GetProductsByName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _productService.GetProductById(id);

            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _productService.SaveProduct(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            // ToDo: cleanup
            var orig = _productService.GetProductById(id);

            if (orig == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }   

            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;

            if (!orig.IsNew)
                _productService.SaveProduct(orig);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _productService.DeleteProduct(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IEnumerable<ProductOption> GetOptions(Guid productId)
        {
            return _productOptionService.GetAllProductOptionsById(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            // ToDo: productId is not used in the method
            var option = _productOptionService.GetProductOptionById(id);

            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;

            _productOptionService.SaveProductOption(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            // ToDo: cleanup
            var orig = _productOptionService.GetProductOptionById(id);

            if (orig == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            orig.Name = option.Name;
            orig.Description = option.Description;

            if (!orig.IsNew)
                _productOptionService.SaveProductOption(orig);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            _productOptionService.DeleteProductOption(id);
        }
    }
}
