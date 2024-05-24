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
    public class WishlistRepoImpl : IWishlistRepo
    {
        private readonly DapperContext _context;

        public WishlistRepoImpl(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetWishlistBooks(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Book>(
                    "sp_GetWishlistBooks",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
                return result.AsList();
            }
        }

        public async Task<Wishlist> AddToWishlist(WishlistRequest wishlistRequest, int userId)
        {
            // Check if the book already exists in the wishlist for the user
            var existingWishlist = await GetWishlistByBookAndUser(wishlistRequest.BookId, userId);
            if (existingWishlist != null)
            {
                // Book already exists in the wishlist, return the existing wishlist item
                return existingWishlist;
            }

            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("BookId", wishlistRequest.BookId);
                parameters.Add("UserId", userId);
                parameters.Add("WishlistId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_AddToWishlist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var wishlistId = parameters.Get<int>("WishlistId");
                var addedWishlist = new Wishlist { WishlistId = wishlistId, BookId = wishlistRequest.BookId, UserId = userId };
                return addedWishlist;
            }
        }

        private async Task<Wishlist> GetWishlistByBookAndUser(int bookId, int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new { BookId = bookId, UserId = userId };
                var wishlist = await connection.QueryFirstOrDefaultAsync<Wishlist>(
                    "sp_GetWishlistByBookAndUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return wishlist;
            }
        }

        public async Task<bool> DeleteWishlist(int userId, int bookId)
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(
                    "sp_DeleteWishlist",
                    new { UserId = userId, BookId = bookId },
                    commandType: CommandType.StoredProcedure
                );
                return result > 0;
            }
        }
    }
}
