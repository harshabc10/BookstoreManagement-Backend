using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AddressRepoImpl : IAddressRepo
    {
        private readonly DapperContext _context;

        public AddressRepoImpl(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Object>> GetAddresses(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Object>(
                    "sp_GetAddresses",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Address>(
                    "sp_GetAddressById",
                    new { AddressId = addressId },
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task AddAddress(Address address)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Address", address.address);
            parameters.Add("City", address.city);
            parameters.Add("State", address.state);
            parameters.Add("Type", address.type);
            parameters.Add("UserId", address.userId);
            parameters.Add("UserName", address.UserName);
            parameters.Add("UserPhone", address.UserPhone);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("sp_AddAddress", parameters);
            }
        }


        public async Task UpdateAddress(Address address)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                    "sp_UpdateAddress",
                    address,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task DeleteAddress(int addressId)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                    "sp_DeleteAddress",
                    new { AddressId = addressId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
