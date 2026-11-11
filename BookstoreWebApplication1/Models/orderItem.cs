using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; } // foreign key
        // public Book Book { get; set; } = null!;
        // [Required]
        public int OrderId { get; set; } // foreign key
        // public Order Order { get; set; } = null!;
        // [Required]
        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}