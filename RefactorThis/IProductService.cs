using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductService
    {
        Products GetAll();
    }
}
