using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using storefront.APILayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storefront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        }

        [HttpGet, Route("getproducts")]
        public async Task<IActionResult> GetProducts()
        {
            return new OkObjectResult(await _productService.GetProductsAsync());
        }
    }
}
