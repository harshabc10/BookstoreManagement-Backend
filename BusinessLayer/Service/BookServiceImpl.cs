using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class BookServiceImpl : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookServiceImpl(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> AddBook(AddBookRequestDto requestDto)
        {
            var book = new Book
            {
                Title = requestDto.Title,
                Author = requestDto.Author,
                Price = requestDto.Price,
                Description = requestDto.Description,
                ImagePath = requestDto.ImagePath,
                Quantity= requestDto.Quantity
            };

            var addedBook = await _bookRepository.AddBook(book);

            // Now assign the BookId from the added book to the original book object
            book.BookId = addedBook.BookId;

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBooks();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _bookRepository.GetBookById(id);
        }

        public async Task<Book> DeleteBook(int id)
        {
            return await _bookRepository.DeleteBook(id);
        }
    }
}
