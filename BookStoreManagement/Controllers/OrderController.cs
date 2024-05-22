using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System.Security.Claims;

namespace BookStoreManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest orderRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var addedOrder = await _orderService.AddOrder(orderRequest, userId);

            if (addedOrder == null)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Message = "Failed to add order. Please check your input data.",
                    Data = null
                });
            }


            return Ok(new ResponseModel<OrderRequest>
            {
                Message = "Order added successfully.",
                Data = addedOrder
            });
        }



        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orderDetails = await _orderService.GetOrderDetails(userId);
            if (orderDetails == null)
                return NotFound(new { Message = "No orders found for the user." });

            return Ok(orderDetails);
        }

    }
}
