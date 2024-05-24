using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IOrderService
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest, int userId);
        Task<List<OrderDetailsResponse>> GetOrderDetails(int userId);

        //Task<OrderResponse> GetOrderById(int orderId);
    }
}
