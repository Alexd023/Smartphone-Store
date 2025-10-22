using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;
using Smartphone_Store.Services.Interfaces;
using Stripe.Checkout;
using System.Security.Claims;

namespace Smartphone_Store.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        private const string CartSessionKey = "CartSessionId";

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository,
                                   UserManager<ApplicationUser> userManager,
                                   IHttpContextAccessor httpContextAccessor,
                                   IOrderRepository orderRepository,
                                   IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<ShoppingCart> GetCartAsync(ClaimsPrincipal user)
        {
            ShoppingCart cart = null;

            if (user.Identity.IsAuthenticated)
            {
                var appUser = await _userManager.GetUserAsync(user);
                cart = await _shoppingCartRepository.GetCartByUserIdAsync(appUser.Id);

                if (cart == null)
                {
                    cart = new ShoppingCart { UserId = appUser.Id };
                    await _shoppingCartRepository.AddCartAsync(cart);
                }
            }
            else
            {
                var cartId = GetOrCreateSessionCartId();
                cart = await _shoppingCartRepository.GetCartBySessionIdAsync(cartId);

                if (cart == null)
                {
                    cart = new ShoppingCart { SessionId = cartId };
                    await _shoppingCartRepository.AddCartAsync(cart);
                }
            }

            return cart;
        }

        public async Task AddToCartAsync(ClaimsPrincipal user, int productId, int quantity)
        {
            var cart = await GetCartAsync(user);
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductID == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductID = productId,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _shoppingCartRepository.UpdateCartAsync(cart);
        }

        public async Task RemoveFromCartAsync(ClaimsPrincipal user, int productId)
        {
            var cart = await GetCartAsync(user);
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductID == productId);

            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                await _shoppingCartRepository.UpdateCartAsync(cart);
            }
        }

        private string GetOrCreateSessionCartId()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartId = session.GetString(CartSessionKey);
            if (cartId == null)
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString(CartSessionKey, cartId);
            }
            return cartId;
        }

        public async Task TransferCartAsync(string sessionId, string userId)
        {
            var sessionCart = await _shoppingCartRepository.GetCartBySessionIdAsync(sessionId);
            var userCart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (sessionCart != null)
            {
                if (userCart == null)
                {
                    sessionCart.UserId = userId;
                    sessionCart.SessionId = null;
                    await _shoppingCartRepository.UpdateCartAsync(sessionCart);
                }
                else
                {
                    foreach (var item in sessionCart.CartItems)
                    {
                        var userCartItem = userCart.CartItems.FirstOrDefault(i => i.ProductID == item.ProductID);
                        if (userCartItem == null)
                        {
                            userCart.CartItems.Add(new CartItem
                            {
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                ShoppingCartID = userCart.ShoppingCartId
                            });
                        }
                        else
                        {
                            userCartItem.Quantity += item.Quantity;
                        }
                    }
                    await _shoppingCartRepository.RemoveCartAsync(sessionCart);
                    await _shoppingCartRepository.UpdateCartAsync(userCart);
                }
            }
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task UpdateProductStockAsync(ShoppingCart cart)
        {
            foreach (var item in cart.CartItems)
            {
                await _productRepository.UpdateStockAsync(item.ProductID, item.Quantity);
            }
        }

        public async Task<(string sessionUrl, string serializedUserAddress)> CheckoutAsync(ClaimsPrincipal user, string fullName, string email, string phoneNumber, string address, string city, string postalCode)
        {
            var userAddress = new UserAddress
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                PostalCode = postalCode
            };

            var cart = await GetCartAsync(user);

            var productList = cart.CartItems;

            var domain = "https://localhost:7233/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"ShoppingCart/OrderConfirmation",
                CancelUrl = domain + $"ShoppingCart/OrderFailure",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * item.Quantity * 100),
                        Currency = "ron",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name.ToString(),
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            var serializedUserAddress = JsonConvert.SerializeObject(userAddress);

            return (session.Url, serializedUserAddress);
        }

        public async Task ConfirmOrderAsync(ClaimsPrincipal user, UserAddress userAddress)
        {
            var cart = await GetCartAsync(user);
            var appUser = await GetUserAsync(user);

            var order = new Order
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                FullName = userAddress.FullName,
                Email = userAddress.Email,
                PhoneNumber = userAddress.PhoneNumber,
                Address = userAddress.Address,
                City = userAddress.City,
                PostalCode = userAddress.PostalCode,
                OrderDate = DateTime.Now,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductID = ci.ProductID,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList()
            };

            await _orderRepository.AddOrderAsync(order);
            await _shoppingCartRepository.RemoveCartAsync(cart);
            await UpdateProductStockAsync(cart);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(ClaimsPrincipal user)
        {
            var appUser = await GetUserAsync(user);
            return await _orderRepository.GetOrdersByUserIdAsync(appUser.Id);
        }
    }
}