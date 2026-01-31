using System.ComponentModel.DataAnnotations;
using Smartphone_Store.Models;

namespace Smartphone_Store.Models
{
	public class ProductDetailsVm
	{
		public Product Product { get; set; } = default!;
		public List<Review>
	Reviews
		{ get; set; } = new();
		public double AverageRating { get; set; }
		public string? CurrentUserId { get; set; }

		public CreateReviewVm NewReview { get; set; } = new();
	}

	public class CreateReviewVm
	{
		public int ProductId { get; set; }

		[Range(1, 5)]
		public int Rating { get; set; } = 5;

		[Required, StringLength(500)]
		public string Comment { get; set; } = "";
	}
}