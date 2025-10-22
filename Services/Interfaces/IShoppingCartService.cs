using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Smartphone_Store.Models;

namespace Smartphone_Store.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetCartAsync(ClaimsPrincipal user);
        Task AddToCartAsync(ClaimsPrincipal user, int productId, int quantity);
        Task RemoveFromCartAsync(ClaimsPrincipal user, int productId);
        Task TransferCartAsync(string sessionId, string userId);
        Task ConfirmOrderAsync(ClaimsPrincipal user, UserAddress userAddress);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user);
        Task UpdateProductStockAsync(ShoppingCart cart);
        Task<(string sessionUrl, string serializedUserAddress)> CheckoutAsync(ClaimsPrincipal user, string fullName, string email, string phoneNumber, string address, string city, string postalCode);
        Task<IEnumerable<Order>> GetUserOrdersAsync(ClaimsPrincipal user);
    }
}