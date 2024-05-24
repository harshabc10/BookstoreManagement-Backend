using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("GetWishlistBooks")]
        public async Task<ActionResult<ResponseModel<List<Book>>>> GetWishlistBooks()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var wishlistBooks = await _wishlistService.GetWishlistBooks(userId);
            var response = new ResponseModel<List<Book>> { Message = "Retrieved wishlist books successfully", Data = wishlistBooks };
            return Ok(response);
        }

        [HttpPost("AddToWishlist")]
        public async Task<ActionResult<ResponseModel<Wishlist>>> AddToWishlist([FromBody] WishlistRequest wishlistRequest)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var addedWishlist = await _wishlistService.AddToWishlist(wishlistRequest,userId);
            var response = new ResponseModel<Wishlist> { Message = "Added to wishlist successfully", Data = addedWishlist };
            return Ok(response);
        }

        [HttpDelete("DeleteWishlist/{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> DeleteWishlist(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isDeleted = await _wishlistService.DeleteWishlist(userId, id);
            var response = new ResponseModel<bool> { Message = "Deleted from wishlist successfully", Data = isDeleted };
            return Ok(response);
        }
    }
}
