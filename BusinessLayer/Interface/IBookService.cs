using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IBookService
    {
        Task<Book> AddBook(AddBookRequestDto requestDto);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> DeleteBook(int id);
    }
}
