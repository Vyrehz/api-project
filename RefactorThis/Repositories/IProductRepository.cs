using System;
using System.Collections.Generic;
using refactor_this.Models;

namespace refactor_this.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByName(string name);
        Product GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}
