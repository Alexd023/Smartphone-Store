using Smartphone_Store.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Smartphone_Store.Services
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product product, IFormFile[] images, ModelStateDictionary modelState);
        Task<SearchResultsViewModel> SearchProductsAsync(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice);
        Task<bool> UpdateProductAsync(Product product, IFormFileCollection images, ModelStateDictionary modelState, int[] existingImageIds);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task DeleteProductAsync(int id);
    }
}