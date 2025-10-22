using Microsoft.AspNetCore.Identity;

namespace Smartphone_Store.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}