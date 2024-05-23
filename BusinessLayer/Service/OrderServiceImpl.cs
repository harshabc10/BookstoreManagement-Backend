using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IOrderRepo _orderRepository;

        public OrderServiceImpl(IOrderRepo orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> AddOrder(OrderRequest orderRequest, int userId)
        {
            // Assuming AddOrder method in the repository returns OrderDetailsResponse
            return await _orderRepository.AddOrder(orderRequest, userId);
        }

        public async Task<List<OrderDetailsResponse>> GetOrderDetails(int userId)
        {
            return await _orderRepository.GetOrderDetails(userId);
        }


    }
}
