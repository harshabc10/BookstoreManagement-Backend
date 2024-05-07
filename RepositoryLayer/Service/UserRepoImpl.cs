using Dapper;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Microsoft.Extensions.Logging;

namespace RepositoryLayer.Service
{
    public class UserRepoImpl : IUserRepo
    {
        private readonly DapperContext context;
        private readonly ILogger logger; // Logger instance
        public UserRepoImpl(DapperContext contex, ILogger<UserRepoImpl> logger)
        {
            this.context = contex;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        public async Task<int> createUser(UserEntity entity)
        {
            try
            {
                string storedProcedure = "sp_CreateUser";
                var parameters = new
                {
                    UserFirstName = entity.UserFirstName,
                    UserEmail = entity.UserEmail,
                    UserPassword = entity.UserPassword,
                    UserPhone=entity.UserPhone
                };

                var connection = context.CreateConnection();
                int userId = await connection.QueryFirstOrDefaultAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                // Log information message
                logger.Info("User created successfully. UserId: {0}", userId);

                return userId;
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.Error(ex, "Error occurred while creating user.");
                throw; // Rethrow the exception
            }
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            try
            {
                var connection = context.CreateConnection();

                // Call the stored procedure to get a user by email
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);

                // Log information message
                logger.Info("Getting user by email: {0}", email);

                return await connection.QueryFirstAsync<UserEntity>("sp_GetUserByEmail", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.Error(ex, "Error occurred while getting user by email.");
                throw; // Rethrow the exception
            }
        }


        public async Task<int> UpdatePassword(string email, string newPassword)
        {
            try
            {
                var connection = context.CreateConnection();

                // Call the stored procedure to update a user's password
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@Password", newPassword);

                int rowsAffected = await connection.ExecuteAsync("sp_UpdatePassword", parameters, commandType: CommandType.StoredProcedure);

                // Log information message
                logger.Info("Password updated successfully for email: {0}. Rows affected: {1}", email, rowsAffected);

                return rowsAffected;
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.Error(ex, "Error occurred while updating password.");
                throw; // Rethrow the exception
            }
        }

    }
}
