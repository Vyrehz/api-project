using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductOptionService
    {
        ProductOptions GetAllProductOptionsById(Guid productId);

        ProductOption GetProductOptionById(Guid id);

        void SaveProductOption(ProductOption productOption);

        void DeleteProductOption(Guid id);
    }
}
