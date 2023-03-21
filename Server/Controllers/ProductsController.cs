using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MyEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Products.ToListAsync());
        }
    }
}
