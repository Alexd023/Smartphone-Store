using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }

        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public ShoppingCart()
        {
            CartItems = new List<CartItem>();
        }
    }
}