using Smartphone_Store.Models;

namespace Smartphone_Store.Repositories.Interfaces
{
	public interface IWishlistRepository
	{
		Task<bool> ExistsAsync(string userId, int productId);
		Task AddAsync(WishlistItem item);
		Task RemoveAsync(string userId, int productId);
		Task<List<Product>> GetByUserIdAsync(string userId);
	}
}