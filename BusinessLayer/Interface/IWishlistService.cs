using ModelLayer.Entity;
using ModelLayer.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IWishlistService
    {
        Task<List<Book>> GetWishlistBooks(int userId);
        Task<Wishlist> AddToWishlist(WishlistRequest wishlistRequest, int userId);
        Task<bool> DeleteWishlist(int userId, int wishlistId);
    }
}
