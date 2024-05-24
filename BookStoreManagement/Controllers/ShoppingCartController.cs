using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic; // Added for List<T>
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public CartController(IShoppingCartService cartService)
        {
            _shoppingCartService = cartService;
        }

        [HttpGet("GetCartBooks")]
        public async Task<ActionResult<ResponseModel<List<Book>>>> GetCartBooks()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cartBooks = await _shoppingCartService.GetCartBooks(userId);
            var response = new ResponseModel<List<Book>> { Message = "Retrieved books successfully", Data = cartBooks };
            return Ok(response);
        }

        /*[HttpGet("GetCartBooks")]
        public async Task<ActionResult<ResponseModel<List<BookWithQuantity>>>> GetCartBooks()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cartBooks = await _shoppingCartService.GetCartBooksWithQuantities(userId);

            var booksWithQuantities = cartBooks.Select(item => new BookWithQuantity
            {
                Book = item.Book,
                Quantity = item.Quantity // Assuming the quantity is fetched from the cart item
            }).ToList();

            var response = new ResponseModel<List<BookWithQuantity>> { Message = "Retrieved books with quantities successfully", Data = booksWithQuantities };
            return Ok(response);
        }*/


        [HttpPost("AddToCart")]
        public async Task<ActionResult<ResponseModel<CartRequest>>> AddToCart([FromBody] CartRequest cartRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updatedCart = await _shoppingCartService.AddToCart(cartRequest, userId);

            // Get the added book from the updatedCart
            var addedBook = updatedCart.FirstOrDefault(b => b.BookId == cartRequest.BookId);
            if (addedBook == null)
            {
                // Book not found in the cart, return an error response
                var errorResponse = new ResponseModel<CartRequest>
                {
                    Success = false,
                    Message = "Error adding the book to cart. Book not found in the cart.",
                    Data = cartRequest // Return the original CartRequest in the response
                };
                return BadRequest(errorResponse);
            }

            // Create the response model with the added book's quantity
            var response = new ResponseModel<Book>
            {
                Message = "Added to cart successfully",
                Data = new Book
                {
                    BookId = cartRequest.BookId,
                    Quantity = cartRequest.Quantity // Use the quantity of the added book
                }
            };
            return Ok(response);
        }


        [HttpPut("UpdateQuantity")]
        public async Task<ActionResult<ResponseModel<CartRequest>>> UpdateQuantity([FromBody] CartRequest cartRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updatedCartRequest = await _shoppingCartService.UpdateQuantity(userId, cartRequest);
            var response = new ResponseModel<CartRequest> { Message = "Updated quantity successfully", Data = updatedCartRequest };
            return Ok(response);
        }

        [HttpDelete("DeleteCart")]
        public async Task<ActionResult<ResponseModel<bool>>> DeleteCart(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isDeleted = await _shoppingCartService.DeleteCart(userId, id);
            var response = new ResponseModel<bool> { Message = "Deleted from cart successfully", Data = isDeleted };
            return Ok(response);
        }
    }
}
