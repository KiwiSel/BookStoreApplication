using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        // public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
