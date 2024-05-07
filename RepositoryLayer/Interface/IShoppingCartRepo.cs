using ModelLayer.Entity;
using ModelLayer.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IShoppingCartRepo
    {
        Task<List<Book>> GetCartBooks(int userId);
        Task<List<Book>> AddToCart(CartRequest cartRequest, int userId);
        /* Task<double> GetPrice(int userId);*/
        Task<CartRequest> UpdateQuantity(int userId, CartRequest cartRequest);
        Task<bool> DeleteCart(int userId,int id);
    }
}
