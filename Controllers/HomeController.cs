using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Models;
using Smartphone_Store.Services;

namespace Smartphone_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice)
        {
            var viewModel = await _productService.SearchProductsAsync(query, brand, minPrice, maxPrice, sortOrder, isUserSpecifiedMinPrice, isUserSpecifiedMaxPrice);
            return View(viewModel);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)

            {

                return NotFound();

            }
            return View(product);
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