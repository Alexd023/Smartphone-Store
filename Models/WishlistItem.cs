using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
	public class WishlistItem
	{
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; } = default!;

		[Required]
		public int ProductId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Product Product { get; set; } = default!;
	}
}