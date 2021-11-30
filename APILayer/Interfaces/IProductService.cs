using storefront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storefront.APILayer.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
    }
}
