using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int ShoppingCartID { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}