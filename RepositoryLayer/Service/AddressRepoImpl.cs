using Dapper;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Address>> GetAddresses(int userId)
        {
            var sql = "SELECT * FROM Addresses WHERE UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Address>(sql, new { UserId = userId });
            }
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            var sql = "SELECT * FROM Addresses WHERE AddressId = @AddressId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Address>(sql, new { AddressId = addressId });
            }
        }

        public async Task AddAddress(Address address)
        {
            var sql = @"INSERT INTO Addresses (Address, City, State, Type, UserId) 
                    VALUES (@Address, @City, @State, @Type, @UserId)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, address);
            }
        }

        public async Task UpdateAddress(Address address)
        {
            var sql = @"UPDATE Addresses 
                    SET Address = @Address, City = @City, State = @State, Type = @Type 
                    WHERE AddressId = @AddressId and UserId=@UserId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, address);
            }
        }

        public async Task DeleteAddress(int addressId)
        {
            var sql = "DELETE FROM Addresses WHERE AddressId = @AddressId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { AddressId = addressId });
            }
        }
    }

}
