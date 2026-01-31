using Microsoft.EntityFrameworkCore;
using Smartphone_Store.Models;
using Smartphone_Store.Repositories.Interfaces;

namespace Smartphone_Store.Repositories
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly ApplicationDbContext _context;

		public ReviewRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Review review)
		{
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
		}

		public Task<List<Review>> GetByProductIdAsync(int productId)
			=> _context.Reviews
				.Where(r => r.ProductId == productId)
				.OrderByDescending(r => r.CreatedAt)
				.ToListAsync();

		public Task<Review?> GetByIdAsync(int id)
			=> _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

		public async Task DeleteAsync(Review review)
		{
			_context.Reviews.Remove(review);
			await _context.SaveChangesAsync();
		}
	}
}