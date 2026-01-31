using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;
using Smartphone_Store.Services.Interfaces;

namespace Smartphone_Store.Services
{
	public class WishlistService : IWishlistService
	{
		private readonly IWishlistRepository _repository;

		public WishlistService(IWishlistRepository repository)
		{
			_repository = repository;
		}

		public async Task AddAsync(string userId, int productId)
		{
			if (await _repository.ExistsAsync(userId, productId))
				return;

			await _repository.AddAsync(new WishlistItem
			{
				UserId = userId,
				ProductId = productId
			});
		}

		public Task RemoveAsync(string userId, int productId)
			=> _repository.RemoveAsync(userId, productId);

		public Task<List<Product>> GetAsync(string userId)
			=> _repository.GetByUserIdAsync(userId);
	}
}