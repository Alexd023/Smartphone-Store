using Microsoft.EntityFrameworkCore;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;

namespace Smartphone_Store.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                                 .Include(c => c.CartItems)
                                 .ThenInclude(i => i.Product)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<ShoppingCart> GetCartBySessionIdAsync(string sessionId)
        {
            return await _context.ShoppingCarts
                                 .Include(c => c.CartItems)
                                 .ThenInclude(i => i.Product)
                                 .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task AddCartAsync(ShoppingCart cart)
        {
            await _context.ShoppingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            _context.ShoppingCarts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartAsync(ShoppingCart cart)
        {
            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }
}