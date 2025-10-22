using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Services.Interfaces;
using Smartphone_Store.Models;
using Smartphone_Store.Services;
using Stripe.Checkout;
using Stripe.Climate;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Smartphone_Store.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _shoppingCartService.GetCartAsync(User);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            await _shoppingCartService.AddToCartAsync(User, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _shoppingCartService.RemoveFromCartAsync(User, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(string fullName, string email, string PhoneNumber, string address, string city, string postalCode)
        {
            var (sessionUrl, serializedUserAddress) = await _shoppingCartService.CheckoutAsync(User, fullName, email, PhoneNumber, address, city, postalCode);
            TempData["UserAddress"] = serializedUserAddress;
            Response.Headers.Add("Location", sessionUrl);
            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmation()
        {
            var userAddressJson = TempData["UserAddress"] as string;
            var userAddress = JsonConvert.DeserializeObject<UserAddress>(userAddressJson);
            await _shoppingCartService.ConfirmOrderAsync(User, userAddress);
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Orders()
        {
            var orders = await _shoppingCartService.GetUserOrdersAsync(User);
            return View("/Views/Home/Orders.cshtml", orders);
        }
    }
}