using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
	public class Review
	{
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; } = default!;

		[Required]
		public int ProductId { get; set; }

		[Range(1, 5)]
		public int Rating { get; set; } = 5;

		[Required, StringLength(500)]
		public string Comment { get; set; } = "";

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Product Product { get; set; } = default!;
	}
}