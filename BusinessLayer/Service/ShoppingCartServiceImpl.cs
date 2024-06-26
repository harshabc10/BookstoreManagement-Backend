﻿using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class ShoppingCartServiceImpl : IShoppingCartService
    {
        private readonly IShoppingCartRepo _shoppingCartRepository;

        public ShoppingCartServiceImpl(IShoppingCartRepo shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<List<Book>> GetCartBooks(int userId)
        {
            return await _shoppingCartRepository.GetCartBooks(userId);
        }

/*        public async Task<List<BookWithQuantity>> GetCartBooksWithQuantities(int userId)
        {
            var cartItems = await _shoppingCartRepository.GetCartItemsByUserId(userId);

            return cartItems.Select(item => new BookWithQuantity
            {
                Book = item.Book, // Assuming you have a Book property in your CartItem model
                Quantity = item.Quantity // Assuming quantity is part of the cart item
            }).ToList();
        }*/


        public async Task<List<Book>> AddToCart(CartRequest cartRequest, int userId)
        {
            return await _shoppingCartRepository.AddToCart(cartRequest, userId);
        }

        public async Task<CartRequest> UpdateQuantity(int userId, CartRequest cartRequest)
        {
            return await _shoppingCartRepository.UpdateQuantity(userId, cartRequest);
        }

        public async Task<bool> DeleteCart(int userId, int id)
        {
            return await _shoppingCartRepository.DeleteCart(userId, id);
        }
    }
}
