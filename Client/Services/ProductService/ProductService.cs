
using System.Net.Http.Json;

namespace MyEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> Products { get; set; }
        public string Message { get; set; } = "Loading Products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProductById(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/products/{productId}");
            return result;
        }

        public async Task GetProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/products");
            if (result != null && result.Data != null)
                Products = result.Data;
        }
        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/products/featured") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/products/category/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No Products found";


            ProductsChanged?.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string term)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/products/searchsuggestion/{term}");
            return result.Data;
        }

        public async Task SearchProduct(string term, int page)
        {
            LastSearchText = term;
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/products/search/{term}/{page}");
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            if (Products.Count == 0)
                Message = "No Products found.";
            ProductsChanged?.Invoke();
        }
    }
}
