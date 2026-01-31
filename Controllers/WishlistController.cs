using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Models;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Controllers
{
	[Authorize]
	public class WishlistController : Controller
	{
		private readonly IWishlistService _wishlistService;
		private readonly UserManager<ApplicationUser> _userManager;

		public WishlistController(IWishlistService wishlistService, UserManager<ApplicationUser> userManager)
		{
			_wishlistService = wishlistService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var userId = _userManager.GetUserId(User)!;
			var products = await _wishlistService.GetAsync(userId);
			return View(products);
		}

		[HttpPost]
		public async Task<IActionResult> Add(int productId)
		{
			var userId = _userManager.GetUserId(User)!;
			await _wishlistService.AddAsync(userId, productId);
			return RedirectToAction("ProductDetails", "Home", new { id = productId });
		}

		[HttpPost]
		public async Task<IActionResult> Remove(int productId)
		{
			var userId = _userManager.GetUserId(User)!;
			await _wishlistService.RemoveAsync(userId, productId);
			return RedirectToAction(nameof(Index));
		}
	}
}