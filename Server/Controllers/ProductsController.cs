using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MyEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> Get()
        {
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] 
        public async  Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id)
        {
            var result = await _productService.GetProductAsync(id);
            return Ok(result);
            
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductCategory(string categoryUrl)
        {
            var result= await _productService.GeProductsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{term}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProducts(string term,int page=1)
        {
            var result = await _productService.SearchProduct(term,page);
            return Ok(result);
        }

        [HttpGet("searchsuggestion/{term}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestion(string term)
        {
            var result = await _productService.SearchProductSuggestion(term);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }

    }
}
