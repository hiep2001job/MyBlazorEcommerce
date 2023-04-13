using MyEcommerce.Shared;
using System.Reflection.Metadata.Ecma335;

namespace MyEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GeProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products
                .Include(p => p.Variants)
                .Where(x => x.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .ToListAsync()
            };
            return response;

        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products
                .Where(p=>p.Featured)
               .Include(p => p.Variants)
               .ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product does not exist.";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products
                .Include(p => p.Variants)
                .ToListAsync()
            };
            return response;
        }
        public async Task<ServiceResponse<ProductSearchResult>> SearchProduct(string term,int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductBySearchText(term)).Count / pageResults);
            var products = await _context.Products
                .Where(
                    p => p.Title.ToLower().Contains(term.ToLower()) ||
                        p.Description.ToLower().Contains(term.ToLower())
                )
                .Include(p => p.Variants)
                .Skip((page - 1)*(int) pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages=(int)pageCount

                }
            };
            return response;

        }

        public async Task<ServiceResponse<List<string>>> SearchProductSuggestion(string term)
        {
            var products = await FindProductBySearchText(term);
            var result = new List<string>();
            foreach (var product in products)
            {
                if (product.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }
                if (product.Description != null)
                {
                    var punctuation=product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(x => x.Trim(punctuation));
                    foreach (var word in words)
                    {
                        if (word.Contains(term, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word)) 
                        {
                            result.Add(word);
                        }
                    }
                }
            }
            return new ServiceResponse<List<string>> { Data = result };
        }

        private async Task<List<Product>> FindProductBySearchText(string term)
        {
            return await _context.Products
                .Where(
                    p => p.Title.ToLower().Contains(term.ToLower()) ||
                        p.Description.ToLower().Contains(term.ToLower())
                )
                .Include(p => p.Variants)
                .ToListAsync();
        }
    }
}
