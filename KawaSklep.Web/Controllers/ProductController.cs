using KawaSklep.Services.Product;
using KawaSklep.Web.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace KawaSklep.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("GetProduct() called");
            var products = _productService.GetAllProducts();
            
            var productViewModels = products
                .Select(ProductMapper.SerializeProductModel);
            
            return Ok(productViewModels);
        }
    }
}
