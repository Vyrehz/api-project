using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductOptionService
    {
        Task<IEnumerable<ProductOption>> GetAllProductOptionsByIdAsync(Guid productId);

        Task<ProductOption> GetProductOptionByIdAsync(Guid productId, Guid id);

        Task SaveProductOptionAsync(ProductOption productOption);

        Task UpsertProductOptionAsync(Guid productId, Guid id, ProductOption updatedProduct);

        Task DeleteProductOptionAsync(Guid productId, Guid id);
    }
}
