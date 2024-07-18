using System;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductOptionService : IProductOptionService
    {
        public ProductOptions GetAllProductOptionsById(Guid productId)
        {
            return new ProductOptions(productId);
        }

        public ProductOption GetProductOptionById(Guid id)
        {
            return new ProductOption(id);
        }

        public void SaveProductOption(ProductOption productOption)
        {
            productOption.Save();
        }

        public void DeleteProductOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}