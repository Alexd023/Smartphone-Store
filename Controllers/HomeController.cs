using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Smartphone_Store.Models;
using Smartphone_Store.Services;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductService _productService;
		private readonly IReviewService _reviewService;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(
			IProductService productService,
			IReviewService reviewService,
			UserManager<ApplicationUser> userManager)
		{
			_productService = productService;
			_reviewService = reviewService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice)
		{
			var viewModel = await _productService.SearchProductsAsync(query, brand, minPrice, maxPrice, sortOrder, isUserSpecifiedMinPrice, isUserSpecifiedMaxPrice);
			return View(viewModel);
		}

		public async Task<IActionResult> ProductDetails(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product is null)
				return NotFound();

			var reviews = await _reviewService.GetForProductAsync(id);

			var vm = new ProductDetailsVm
			{
				Product = product,
				Reviews = reviews,
				AverageRating = reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating),
				CurrentUserId = _userManager.GetUserId(User),
				NewReview = new CreateReviewVm { ProductId = id }
			};

			return View(vm);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult AddProduct()
		{
			return View("/Views/Admin/AddProduct.cshtml");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddProduct(Product model, IFormFile[] images)
		{
			if (await _productService.AddProductAsync(model, images, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}

			return View("/Views/Admin/AddProduct.cshtml", model);
		}
	}
}