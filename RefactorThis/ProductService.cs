using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductService : IProductService
    {
        public Products GetAll()
        {
            return new Products();
        }
    }
}