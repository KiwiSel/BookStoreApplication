using System.ComponentModel.DataAnnotations;

namespace BookstoreWebApplication1.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        [Required]
        public string ISBN { get; set; } = null!;
        [Required]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public BookFormat Format { get; set; }
        public BookGenre Genre { get; set; }


    }
}
