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
            var response = new ResponseModel<List<Book>> {Message="Retrieved books successfully", Data = cartBooks };
            return Ok(response);
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<ResponseModel<List<Book>>>> AddToCart([FromBody] CartRequest cartRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var updatedCart = await _shoppingCartService.AddToCart(cartRequest, userId);
            var response = new ResponseModel<List<Book>> { Message = "Added to cart successfully", Data = updatedCart };
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
        public async Task<ActionResult<ResponseModel<bool>>> DeleteCart([FromBody] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isDeleted = await _shoppingCartService.DeleteCart(userId, id);
            var response = new ResponseModel<bool> { Message = "Deleted from cart successfully", Data = isDeleted };
            return Ok(response);
        }
    }
}
