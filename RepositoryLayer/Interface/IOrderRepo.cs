using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IOrderRepo
    {
        Task<OrderRequest> AddOrder(OrderRequest order, int userId);
        Task<List<OrderDetailsResponse>> GetOrderDetails(int userId);

    }
}
