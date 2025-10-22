using Smartphone_Store.ViewModels;
using Smartphone_Store.Models;

namespace Smartphone_Store.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<ManageUserRolesViewModel>> GetUserRolesAsync();
        Task<ManageUserRolesViewModel> GetUserRoleAsync(string userId);
        Task UpdateUserRolesAsync(ManageUserRolesViewModel model);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task DeleteOrderAsync(int orderId);
    }
}