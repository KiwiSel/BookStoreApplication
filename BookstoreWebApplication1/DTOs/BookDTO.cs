using BookstoreWebApplication1.Models;

namespace BookstoreWebApplication1.DTOs
{
    public class BookDTO
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }
        public BookFormat? Format { get; set; }
        public BookGenre? Genre { get; set; }
    }
}