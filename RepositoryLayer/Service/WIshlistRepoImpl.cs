using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Collections.Generic;
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
            var query = @"
                SELECT b.* 
                FROM Wishlist w
                JOIN Books b ON w.BookId = b.BookId
                WHERE w.UserId = @UserId";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Book>(query, new { UserId = userId });
                return result.AsList();
            }
        }

        public async Task<Wishlist> AddToWishlist(WishlistRequest wishlistRequest, int userId)
        {
            var query = @"
                INSERT INTO Wishlist (BookId, UserId) 
                VALUES (@BookId, @UserId); 
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = _context.CreateConnection())
            {
                var parameters = new { BookId = wishlistRequest.BookId, UserId = userId };
                var wishlistId = await connection.ExecuteScalarAsync<int>(query, parameters);
                var addedWishlist = new Wishlist { WishlistId = wishlistId, BookId = wishlistRequest.BookId, UserId = userId };
                return addedWishlist;
            }
        }

        public async Task<bool> DeleteWishlist(int userId, int wishlistId)
        {
            var query = "DELETE FROM Wishlist WHERE UserId = @UserId AND WishlistId = @WishlistId";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { UserId = userId, WishlistId = wishlistId });
                return result > 0;
            }
        }
    }
}
