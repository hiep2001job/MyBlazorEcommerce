namespace MyEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int productId);

        Task<ServiceResponse<List<Product>>> GeProductsByCategory(string categoryUrl);
        Task<ServiceResponse<ProductSearchResult>> SearchProduct(string term,int page);
        Task<ServiceResponse<List<string>>> SearchProductSuggestion(string term);
        Task<ServiceResponse<List<Product>>> GetFeaturedProducts();

    }
}
