using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        
        [Required]
        public int UserId { get; set; } //foreign key
        // public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();
    

    }
}
