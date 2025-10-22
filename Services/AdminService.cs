using Microsoft.AspNetCore.Identity;
using Smartphone_Store.Models;
using Smartphone_Store.Services.Interfaces;
using Smartphone_Store.ViewModels;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminService(IAdminRepository adminRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _adminRepository = adminRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<ManageUserRolesViewModel>> GetUserRolesAsync()
    {
        var users = await _adminRepository.GetAllUsersAsync();
        var userRolesViewModel = new List<ManageUserRolesViewModel>();

        foreach (var user in users)
        {
            var thisViewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = new List<UserRoleViewModel>()
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    RoleName = role,
                    IsSelected = true
                };
                thisViewModel.Roles.Add(userRoleViewModel);
            }

            userRolesViewModel.Add(thisViewModel);
        }

        return userRolesViewModel;
    }

    public async Task<ManageUserRolesViewModel> GetUserRoleAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return null;
        }

        var model = new ManageUserRolesViewModel
        {
            UserId = user.Id,
            Email = user.Email,
            Roles = new List<UserRoleViewModel>()
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in _roleManager.Roles)
        {
            var userRoleViewModel = new UserRoleViewModel
            {
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name)
            };
            model.Roles.Add(userRoleViewModel);
        }

        return model;
    }

    public async Task UpdateUserRolesAsync(ManageUserRolesViewModel model)
    {
        var user = await _adminRepository.GetUserByIdAsync(model.UserId);
        if (user == null)
        {
            return;
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = model.Roles.Where(x => x.IsSelected && !string.IsNullOrEmpty(x.RoleName)).Select(y => y.RoleName).ToList();

        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _adminRepository.GetAllOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _adminRepository.GetOrderByIdAsync(id);
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        await _adminRepository.DeleteOrderAsync(orderId);
    }
}