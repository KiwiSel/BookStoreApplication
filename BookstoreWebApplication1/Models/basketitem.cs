using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; } // foreign key
        [Required]
        public int BasketId { get; set; } // foreign key
        public int Quantity { get; set; }
    }
}