using Smartphone_Store.Models;

namespace Smartphone_Store.Services.Interfaces
{
	public interface IWishlistService
	{
		Task AddAsync(string userId, int productId);
		Task RemoveAsync(string userId, int productId);
		Task<List<Product>> GetAsync(string userId);
	}
}