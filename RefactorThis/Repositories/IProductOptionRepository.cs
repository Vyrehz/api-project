using System;
using System.Collections.Generic;
using refactor_this.Models;

namespace refactor_this.Repositories
{
    public interface IProductOptionRepository
    {
        IEnumerable<ProductOption> GetAllByProductId(Guid productId);
        ProductOption GetById(Guid productId, Guid id);
        void Add(ProductOption option);
        void Update(ProductOption option);
        void Delete(Guid id);
    }
}
