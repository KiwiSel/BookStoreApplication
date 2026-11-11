using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class Basket
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } //foreign key

        public List<BasketItem> BasketItems { get; set; } = new();
    

    }
}
