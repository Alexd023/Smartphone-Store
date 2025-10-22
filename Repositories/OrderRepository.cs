using Microsoft.EntityFrameworkCore;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;

namespace Smartphone_Store.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(ci => ci.Product)
                                 .Where(o => o.UserId == userId)
                                 .ToListAsync();
        }
    }
}
