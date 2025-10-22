using Smartphone_Store.Models;

namespace Smartphone_Store.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetCartByUserIdAsync(string userId);
        Task<ShoppingCart> GetCartBySessionIdAsync(string sessionId);
        Task AddCartAsync(ShoppingCart cart);
        Task UpdateCartAsync(ShoppingCart cart);
        Task RemoveCartAsync(ShoppingCart cart);
    }
}
