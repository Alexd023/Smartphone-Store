using Smartphone_Store.Models;

namespace Smartphone_Store.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task<SearchResultsViewModel> SearchProductsAsync(string query, string[] brand, decimal? minPrice, decimal? maxPrice, string sortOrder, bool isUserSpecifiedMinPrice, bool isUserSpecifiedMaxPrice);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateStockAsync(int productId, int quantity);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}