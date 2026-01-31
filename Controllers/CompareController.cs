using Microsoft.AspNetCore.Mvc;
using Smartphone_Store.Services;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Controllers
{
	public class CompareController : Controller
	{
		private const string Key = "compare_ids";
		private const int MaxItems = 3;

		private readonly IProductService _productService;

		public CompareController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			var ids = GetIds();

			// ia produsele în ordinea ids (ca să fie stabil)
			var products = new List<Models.Product>();
			foreach (var id in ids)
			{
				var p = await _productService.GetProductByIdAsync(id);
				if (p != null) products.Add(p);
			}

			return View(products);
		}

		[HttpPost]
		public IActionResult Add(int productId)
		{
			var ids = GetIds();

			if (!ids.Contains(productId) && ids.Count < MaxItems)
			{
				ids.Add(productId);
				SaveIds(ids);
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult Remove(int productId)
		{
			var ids = GetIds();
			ids.Remove(productId);
			SaveIds(ids);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult Clear()
		{
			SaveIds(new List<int>());
			return RedirectToAction(nameof(Index));
		}

		private List<int> GetIds()
		{
			var raw = HttpContext.Session.GetString(Key);
			if (string.IsNullOrWhiteSpace(raw))
				return new List<int>();

			return raw.Split(',')
				.Select(int.Parse)
				.Distinct()
				.ToList();
		}

		private void SaveIds(List<int> ids)
			=> HttpContext.Session.SetString(Key, string.Join(",", ids));
	}
}