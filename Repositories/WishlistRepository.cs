using Microsoft.EntityFrameworkCore;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;

namespace Smartphone_Store.Repositories
{
	public class WishlistRepository : IWishlistRepository
	{
		private readonly ApplicationDbContext _context;

		public WishlistRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public Task<bool> ExistsAsync(string userId, int productId)
			=> _context.WishlistItems.AnyAsync(w => w.UserId == userId && w.ProductId == productId);

		public async Task AddAsync(WishlistItem item)
		{
			_context.WishlistItems.Add(item);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveAsync(string userId, int productId)
		{
			var item = await _context.WishlistItems
				.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

			if (item is null) return;

			_context.WishlistItems.Remove(item);
			await _context.SaveChangesAsync();
		}

		public Task<List<Product>> GetByUserIdAsync(string userId)
			=> _context.WishlistItems
				.Where(w => w.UserId == userId)
				.OrderByDescending(w => w.CreatedAt)
				.Select(w => w.Product)
				.ToListAsync();
	}
}
