using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        public Task<int> createUser(UserRequest request);
        public Task<UserResponse> Login(String Email, String password);
        public Task<String> ChangePasswordRequest(String Email);
        Task<string> ChangePassword(string otp, String password);
    }
}
