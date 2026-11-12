using BookstoreWebApplication1.Models;

namespace BookstoreWebApplication1.DTOs
{
    public class OrderDTO
    {
        public DateTime? OrderDate { get; set; }
        public int? UserId { get; set; } //foreign key
        // public User User { get; set; } = null!;
        public List<OrderItem>? OrderItems { get; set; } = new();
        public double? Price { get; set; }
    }
}