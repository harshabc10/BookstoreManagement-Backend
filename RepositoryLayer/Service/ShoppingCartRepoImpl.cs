using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class ShoppingCartRepository : IShoppingCartRepo
    {
        private readonly DapperContext _context;

        public ShoppingCartRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetCartBooks(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var cartBooks = await connection.QueryAsync<Book>(
                    "sp_GetCartBooks", 
                    new { UserId = userId }, 
                    commandType: CommandType.StoredProcedure
                );
                return cartBooks.ToList();
            }
        }

        public async Task<List<Book>> AddToCart(CartRequest cartRequest, int userId)
        {
            bool isBookInCart = await IsBookInCart(cartRequest.BookId, userId);

            if (!isBookInCart)
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(
                        "sp_AddToCart", 
                        new { UserId = userId, BookId = cartRequest.BookId, Quantity = cartRequest.Quantity }, 
                        commandType: CommandType.StoredProcedure
                    );

                    return await GetCartBooks(userId);
                }
            }
            else
            {
                return await GetCartBooks(userId);
            }
        }

        private async Task<bool> IsBookInCart(int bookId, int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                int count = await connection.ExecuteScalarAsync<int>(
                    "sp_IsBookInCart", 
                    new { UserId = userId, BookId = bookId }, 
                    commandType: CommandType.StoredProcedure
                );
                return count > 0;
            }
        }

        public async Task<CartRequest> UpdateQuantity(int userId, CartRequest cartRequest)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                    "sp_UpdateQuantity", 
                    new { Quantity = cartRequest.Quantity, UserId = userId, BookId = cartRequest.BookId }, 
                    commandType: CommandType.StoredProcedure
                );

                return cartRequest;
            }
        }

        public async Task<bool> DeleteCart(int userId, int id)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                    "sp_DeleteCart", 
                    new { UserId = userId, BookId = id }, 
                    commandType: CommandType.StoredProcedure
                );

                return !(await IsBookInCart(id, userId));
            }
        }

        public async Task<List<BookWithQuantity>> GetCartItemsByUserId(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var cartItems = await connection.QueryAsync<BookWithQuantity>(
                    "sp_GetCartItemsByUserId", 
                    new { UserId = userId }, 
                    commandType: CommandType.StoredProcedure
                );
                return cartItems.ToList();
            }
        }
    }
}

