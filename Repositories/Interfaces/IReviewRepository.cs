using Smartphone_Store.Models;

namespace Smartphone_Store.Repositories.Interfaces
{
	public interface IReviewRepository
	{
		Task AddAsync(Review review);
		Task<List<Review>> GetByProductIdAsync(int productId);
		Task<Review?> GetByIdAsync(int id);
		Task DeleteAsync(Review review);
	}
}