namespace MyEcommerce.Client.Services.CartService
{
    public interface ICartService
    {
        event Action Onchange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProduct(int productId,int productTypeId);
        Task UpdateQuantity(CartProductResponse product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetItemsCount();
    }
}
