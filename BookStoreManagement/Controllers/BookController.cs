using BusinessLayer.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequestDto requestDto)
        {
            try
            {
                var result = await _bookService.AddBook(requestDto);

                if (result != null)
                {
                    var bookResponse = new ResponseModel<AddBookRequestDto>
                    {
                        Message = "Book added successfully.",
                        Data = new AddBookRequestDto
                        {
                            // Assuming 'BookResponse' is the response model for adding books
                            Title = requestDto.Title,
                            Author = requestDto.Author,
                            Price = requestDto.Price,
                            Description = requestDto.Description,
                            ImagePath = requestDto.ImagePath,
                            Quantity = requestDto.Quantity
                        }
                    };

                    return Ok(bookResponse);
                }
                else
                {
                    return StatusCode(500, "Failed to add book.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooks();

                if (books != null && books.Any())
                {
                    var bookResponse = books.Select(book => new Book
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        Description = book.Description,
                        Quantity = book.Quantity,
                        ImagePath = book.ImagePath
                    }).ToList();

                    return Ok(new ResponseModel<List<Book>>
                    {
                        Message = "Books retrieved successfully.",
                        Data = bookResponse
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Message = "No books found."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<object>
                {
                    Message = $"Error: {ex.Message}"
                });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var result = await _bookService.DeleteBook(id);

                if (result != null)
                {
                    var bookResponse = new ResponseModel<AddBookRequestDto>
                    {
                        Message = "Book deleted successfully.",
                    };

                    return Ok(bookResponse);
                }
                else
                {
                    return StatusCode(500, "Failed to delete book.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var result = await _bookService.GetBookById(id);

                if (result != null)
                {
                    var bookResponse = new ResponseModel<Book>
                    {
                        Message = "Book retrieved successfully.",
                        Data = result
                    };

                    return Ok(bookResponse);
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Message = "Book not found."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<object>
                {
                    Message = $"Error: {ex.Message}"
                });
            }
        }


    }
}
