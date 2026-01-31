using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Services
{
	public class ReviewServicee : IReviewService
	{
		private readonly IReviewRepository _repo;

		public ReviewServicee(IReviewRepository repo)
		{
			_repo = repo;
		}

		public Task AddAsync(string userId, int productId, int rating, string comment)
			=> _repo.AddAsync(new Review
			{
				UserId = userId,
				ProductId = productId,
				Rating = rating,
				Comment = comment.Trim()
			});

		public Task<List<Review>> GetForProductAsync(int productId)
			=> _repo.GetByProductIdAsync(productId);

		public async Task<bool> DeleteAsync(string userId, int reviewId)
		{
			var review = await _repo.GetByIdAsync(reviewId);
			if (review is null) return false;
			if (review.UserId != userId) return false;

			await _repo.DeleteAsync(review);
			return true;
		}
	}
}