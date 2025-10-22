using Microsoft.AspNetCore.Identity;
using Smartphone_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAdminRepository
{
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task DeleteOrderAsync(int orderId);
}