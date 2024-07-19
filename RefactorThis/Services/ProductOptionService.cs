using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Repositories;

namespace refactor_this.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductOptionService(IProductOptionRepository productOptionRepository)
        {
            _productOptionRepository = productOptionRepository;
        }

        public async Task<IEnumerable<ProductOption>> GetAllProductOptionsByIdAsync(Guid productId)
        {
            return await _productOptionRepository.GetAllByProductIdAsync(productId);
        }

        public async Task<ProductOption> GetProductOptionByIdAsync(Guid productId, Guid id)
        {
            return await _productOptionRepository.GetByIdAsync(productId, id);
        }

        public async Task SaveProductOptionAsync(ProductOption productOption)
        {
            if (productOption.IsNew)
            {
                await _productOptionRepository.AddAsync(productOption);
            }
            else
            {
                await _productOptionRepository.UpdateAsync(productOption);
            }
        }

        public async Task UpsertProductOptionAsync(Guid productId, Guid id, ProductOption updatedProductOption)
        {
            var product = await GetProductOptionByIdAsync(productId, id);

            product.Name = updatedProductOption.Name;
            product.Description = updatedProductOption.Description;

            await SaveProductOptionAsync(product);
        }

        public async Task DeleteProductOptionAsync(Guid productId, Guid id)
        {
            var opt = await _productOptionRepository.GetByIdAsync(productId, id);

            if (opt == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            await _productOptionRepository.DeleteAsync(id);
        }
    }
}