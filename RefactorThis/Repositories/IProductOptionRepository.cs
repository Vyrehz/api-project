using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Repositories
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetAllByProductIdAsync(Guid productId);
        Task<ProductOption> GetByIdAsync(Guid productId, Guid id);
        Task AddAsync(ProductOption option);
        Task UpdateAsync(ProductOption option);
        Task DeleteAsync(Guid id);
    }
}
