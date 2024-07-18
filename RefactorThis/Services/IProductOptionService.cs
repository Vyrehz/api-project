﻿using System;
using System.Collections.Generic;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductOptionService
    {
        IEnumerable<ProductOption> GetAllProductOptionsById(Guid productId);

        ProductOption GetProductOptionById(Guid id);

        void SaveProductOption(ProductOption productOption);

        void DeleteProductOption(Guid id);
    }
}
