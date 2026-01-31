using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Models;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Controllers
{
	[Authorize]
	public class ReviewsController : Controller
	{
		private readonly IReviewService _reviewService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ReviewsController(IReviewService reviewService, UserManager<ApplicationUser> userManager)
		{
			_reviewService = reviewService;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind(Prefix = "NewReview")] CreateReviewVm vm)
		{
			if (!ModelState.IsValid)
				return RedirectToAction("ProductDetails", "Home", new { id = vm.ProductId });

			var userId = _userManager.GetUserId(User)!;
			await _reviewService.AddAsync(userId, vm.ProductId, vm.Rating, vm.Comment);

			return RedirectToAction("ProductDetails", "Home", new { id = vm.ProductId });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id, int productId)
		{
			var userId = _userManager.GetUserId(User)!;
			await _reviewService.DeleteAsync(userId, id);

			return RedirectToAction("ProductDetails", "Home", new { id = productId });
		}
	}
}
