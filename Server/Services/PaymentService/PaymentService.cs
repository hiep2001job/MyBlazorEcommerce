using Microsoft.AspNetCore.Components;
using Stripe;
using Stripe.Checkout;
using System.Security.Policy;

namespace MyEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_24392f15ed4a69c472708e2be9e3ece95086e56b8b987f9bb3ca55491ac9bc26";

        public PaymentService(ICartService cartService,
            IAuthService authService,
            IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51MptT0Ai3loYImaGjOxf6704jwZDBIgGnkUNaU1oAUdaGr9IaQAIv2vgO5T1AViTY5IKguMQE18dqaQLuozN13nh00UFGZOtzh";

            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity,
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection=new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries=new List<string> { "US","VN"}
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7196/order-success",
                CancelUrl = "https://localhost:7196/cart"

            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json, 
                        request.Headers["Stripe-Signature"],
                        secret
                    );
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session =stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }
                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {

                return new ServiceResponse<bool> { Data = false, Message = e.Message, Success = false };
            }
        }
    }
}
