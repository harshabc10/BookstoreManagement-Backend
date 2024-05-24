using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ResponseDto
{
    public class BookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }

        public static BookResponse FromCartItem(Book book, int cartQuantity)
        {
            return new BookResponse
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Description = book.Description,
                Quantity = cartQuantity,
                ImagePath = book.ImagePath
            };
        }
    }
}
