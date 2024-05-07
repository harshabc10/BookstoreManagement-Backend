using BuisinessLayer.MailSender;
using BusinessLayer.CustomException;
using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepo UserRepo;
        private static string otp;
        private static string mailid;
        private static UserEntity entity;

        private readonly IConfiguration _configuration;
        public UserServiceImpl(IUserRepo UserRepo, IConfiguration configuration)
        {
            this.UserRepo = UserRepo;
            _configuration = configuration;
        }

        private UserEntity MapToEntity(UserRequest request)
        {
            return new UserEntity
            {
                UserFirstName = request.FirstName,
                UserEmail = request.Email,
                UserPassword = Encrypt(request.Password)
            };
        }

        private String Encrypt(String password)
        {
            byte[] passByte = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passByte);
        }

        private String Decrypt(String encryptedPass)
        {
            byte[] passbyte = Convert.FromBase64String(encryptedPass);
            String res = Encoding.UTF8.GetString(passbyte);
            return res;
        }

        private UserResponse MapToResponce(UserEntity response)
        {
            return new UserResponse
            {
                FirstName = response.UserFirstName,
                Email = response.UserEmail,
                Id = response.UserId
            };
        }

        public async Task<int> createUser(UserRequest request)
        {
            return await UserRepo.createUser(MapToEntity(request));
        }

        public async Task<UserResponse> Login(string Email, string password)
        {
            UserEntity entity;
            try
            {
                entity = await UserRepo.GetUserByEmail(Email);
            }
            catch (AggregateException e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId");
            }

            if (password.Equals(Decrypt(entity.UserPassword)))
            {
                return MapToResponce(entity);
            }
            else
            {
                Console.WriteLine(Decrypt(entity.UserPassword));
                Console.WriteLine(password);
                Console.WriteLine(password.Equals(Decrypt(entity.UserPassword)));
                throw new PasswordMissmatchException("Incorrect Password");
            }
        }

        public async Task<String> ChangePasswordRequest(string Email)
        {
            try
            {
                entity = await UserRepo.GetUserByEmail(Email);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId " + e.Message);
            }

            string generatedotp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                generatedotp += r.Next(0, 10);
            }
            otp = generatedotp;
            mailid = Email;
            MailSenderClass.sendMail(Email, generatedotp);
            Console.WriteLine(otp);
            return "MailSent ✔️";
        }

        public async Task<string> ChangePassword(string otp, string password)
        {
            if (otp == null)
            {
                return "Generate Otp First";
            }

            if (Decrypt(entity.UserPassword).Equals(password))
            {
                throw new PasswordMissmatchException("Don't give the existing password");
            }

            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[a-zA-Z\d!@#$%^&*]{8,16}$"))
            {
                if (UserServiceImpl.otp.Equals(otp))
                {
                    if (await UserRepo.UpdatePassword(mailid, Encrypt(password)) == 1)
                    {
                        entity = null; otp = null; mailid = null;
                        return "Password changed successfully";
                    }
                }
                else
                {
                    return "OTP mismatch";
                }
            }
            else
            {
                return "Password does not meet the criteria";
            }
            return "Password not changed";
        }

    }
}
