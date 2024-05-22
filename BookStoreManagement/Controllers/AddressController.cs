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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
           
        }


        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var addresses = await _addressService.GetAddresses(userId);
            return Ok(new ResponseModel<IEnumerable<AddressWithUserDetails>>
            {
                Message = "Addresses retrieved successfully.",
                Data = addresses
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address == null)
                return NotFound(new ResponseModel<string>
                {
                    Message = "Address not found.",
                    Data = null
                });

            return Ok(new ResponseModel<Address>
            {
                Message = "Address retrieved successfully.",
                Data = address
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressRequest addressRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _addressService.AddAddress(addressRequest, userId);

            // Fetch the added address
            var addedAddress = await _addressService.GetAddresses(userId);

            return Ok(new ResponseModel<IEnumerable<AddressWithUserDetails>>
            {
                Message = "Address added successfully.",
                Data = addedAddress
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressRequest addressRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _addressService.UpdateAddress(id, addressRequest, userId);

            // Fetch the updated address
            var updatedAddress = await _addressService.GetAddressById(id);

            return Ok(new ResponseModel<Address>
            {
                Message = "Address updated successfully.",
                Data = updatedAddress
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _addressService.DeleteAddress(id);

            return Ok(new ResponseModel<string>
            {
                Message = "Address deleted successfully.",
                Data = null
            });
        }
    }
}
