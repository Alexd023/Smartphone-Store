using Smartphone_Store.Models;

namespace Smartphone_Store.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
