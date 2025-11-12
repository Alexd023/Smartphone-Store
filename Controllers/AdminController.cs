

namespace Smartphone_Store.Controllers
{
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Services.Interfaces;
using Smartphone_Store.ViewModels;
using Smartphone_Store.Models;
using Smartphone_Store.Services;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly IProductService _productService;

    public AdminController(IAdminService adminService, IProductService productService)
    {
        _adminService = adminService;
        _productService = productService;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public async Task<IActionResult> RolesIndex()
    {
        var model = await _adminService.GetUserRolesAsync();
        return View(model);
    }

    public async Task<IActionResult> ManageUserRoles(string userId)
    {
        var model = await _adminService.GetUserRoleAsync(userId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel model)
    {
        await _adminService.UpdateUserRolesAsync(model);
        return RedirectToAction(nameof(RolesIndex));
    }

    public async Task<IActionResult> OrderManagement()
    {
        var orders = await _adminService.GetAllOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> ViewOrder(int orderId)
    {
        var order = await _adminService.GetOrderByIdAsync(orderId);
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        await _adminService.DeleteOrderAsync(orderId);
        return RedirectToAction("OrderManagement");
    }

    public async Task<IActionResult> ProductManagement()
    {
        var products = await _productService.GetAllProductsAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)

        {

            return NotFound();

        }
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(int id, Product product, IFormFileCollection images, int[] ExistingImageIds)
    {
        if (!ModelState.IsValid) { return View(product); }
            if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProductAsync(product, images, ModelState, ExistingImageIds);
            if (result)
            {
                return RedirectToAction("ProductManagement");
            }
        }

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("ProductManagement");
    }
}
}
