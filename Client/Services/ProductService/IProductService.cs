namespace MyEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        string Message { get; set; } 
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }

        Task GetProducts();
        Task<ServiceResponse<Product>> GetProductById(int productId);
        Task GetProducts(string? categoryUrl);

        Task SearchProduct(string term, int page);
        Task<List<string>> GetProductSearchSuggestions(string term);
        
    }
}
