using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public decimal TotalAmount => OrderItems.Sum(OrderItem => OrderItem.Price * OrderItem.Quantity);
    }
}
