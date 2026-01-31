using Smartphone_Store.Models;

namespace Smartphone_Store.Services.Interfaces
{
	public interface IReviewService
	{
		Task AddAsync(string userId, int productId, int rating, string comment);
		Task<List<Review>> GetForProductAsync(int productId);
		Task<bool> DeleteAsync(string userId, int reviewId);
	}
}