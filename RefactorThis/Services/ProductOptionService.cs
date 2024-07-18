using System;
using System.Collections.Generic;
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

        public IEnumerable<ProductOption> GetAllProductOptionsById(Guid productId)
        {
            return _productOptionRepository.GetAllByProductId(productId);
        }

        public ProductOption GetProductOptionById(Guid productId, Guid id)
        {
            return _productOptionRepository.GetById(productId, id);
        }

        public void SaveProductOption(ProductOption productOption)
        {
            if (productOption.IsNew)
            {
                _productOptionRepository.Add(productOption);
            }
            else
            {
                _productOptionRepository.Update(productOption);
            }
        }

        public void DeleteProductOption(Guid productId, Guid id)
        {
            var opt = _productOptionRepository.GetById(productId, id);

            if (opt == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            _productOptionRepository.Delete(id);
        }
    }
}