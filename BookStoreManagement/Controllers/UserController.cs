using BusinessLayer.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]

    public class UserController : ControllerBase
    {

       private readonly IUserService service;
        private readonly IConfiguration _configuration;
        

        public UserController(IUserService service, IConfiguration configuration)
        {
            this.service = service;
            _configuration = configuration;
           

        }

        [HttpPost]
        public async Task<IActionResult> createUser(UserRequest request)
        {
            try
            {
                var userId = await service.createUser(request);
                // Assuming service.createUser returns an int

                var userResponse = new ResponseModel<UserResponse>
                {
                    Message = "User created successfully.",
                    Data = new UserResponse // Assuming 'UserResponse' is the response model for user creation
                    {
                        FirstName = request.FirstName,
                        Email = request.Email,
                        Id = userId,
                        Phone=request.Phone
                
                    }
                };

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string password)
        {
            try
            {
                var user = await service.Login(Email, password);

                // Check if user is found
                if (user == null)
                {
                    return NotFound("User not found."); // You can customize this message as needed
                }

                

                // Generate JWT token
                var token = CreateToken(user);

                // Return token along with success message
                var response = new
                {
                    Token = token,
                    UserName = user.FirstName, // Assuming FirstName contains the username
                    Email = user.Email,
                    Message = "Token generated successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            try
            {
                // Assuming service.ForgotPassword returns a string message
                var responseMessage = await service.ChangePasswordRequest(Email);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = responseMessage,
                    Data= Email
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                });
            }
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Otp, string Password)
        {
            try
            {
                // Assuming service.ResetPassword returns a string message
                var responseMessage = await service.ChangePassword(Otp, Password);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = responseMessage
                    
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }



        private string CreateToken(UserResponse user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(100);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
