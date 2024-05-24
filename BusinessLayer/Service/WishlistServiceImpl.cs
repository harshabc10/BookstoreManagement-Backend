using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class WishlistServiceImpl : IWishlistService
    {
        private readonly IWishlistRepo _wishlistRepository;

        public WishlistServiceImpl(IWishlistRepo wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<List<Book>> GetWishlistBooks(int userId)
        {
            return await _wishlistRepository.GetWishlistBooks(userId);
        }

        public async Task<Wishlist> AddToWishlist(WishlistRequest wishlistRequest ,int userId)
        {
            return await _wishlistRepository.AddToWishlist(wishlistRequest,userId);
        }

        public async Task<bool> DeleteWishlist(int userId, int wishlistId)
        {
            return await _wishlistRepository.DeleteWishlist(userId, wishlistId);
        }
    }
}
