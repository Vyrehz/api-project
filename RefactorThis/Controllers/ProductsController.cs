using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Authentication;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("api/products")]
    [ApiKeyAuthentication]
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
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productService.GetAllProductsAsync();
        }

        [Route]
        [HttpGet]
        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            return await _productService.GetProductsByNameAsync(name);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<Product> GetProductAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public async Task CreateAsync(Product product)
        {
            await _productService.SaveProductAsync(product);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task UpdateAsync(Guid id, Product product)
        {
            await _productService.UpsertProductAsync(id, product);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _productService.DeleteProductAsync(id);
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
            var option = _productOptionService.GetProductOptionById(productId, id);

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
        public void UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            _productOptionService.UpsertProductOption(productId, id, option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid productId, Guid id)
        {
            _productOptionService.DeleteProductOption(productId, id);
        }
    }
}
