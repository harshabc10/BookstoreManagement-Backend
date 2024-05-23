using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class OrderRepoImpl : IOrderRepo
    {
        private readonly DapperContext _context;

        public OrderRepoImpl(DapperContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse> AddOrder(OrderRequest order, int userId)
        {
            var queryOrder = "INSERT INTO Orders (AddressId, UserId, OrderDate) VALUES (@AddressId, @UserId, @OrderDate); SELECT CAST(SCOPE_IDENTITY() as int)";
            var queryOrderBook = "INSERT INTO OrderBooks (OrderId, BookId) VALUES (@OrderId, @BookId)";

            using (var connection = _context.CreateConnection() as SqlConnection)
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("Connection is not of type SqlConnection.");
                }

                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var orderId = await connection.ExecuteScalarAsync<int>(queryOrder, new
                        {
                            order.AddressId,
                            UserId = userId,
                            OrderDate = DateTime.UtcNow
                        }, transaction);

                        foreach (var bookId in order.BookIds)
                        {
                            await connection.ExecuteAsync(queryOrderBook, new
                            {
                                OrderId = orderId,
                                BookId = bookId
                            }, transaction);
                        }

                        transaction.Commit();

                        // Return the OrderDetailsResponse with AddressId and BookId
                        return new OrderResponse
                        {
                            OrderId = orderId,
                            AddressId = order.AddressId,
                            BookIds = order.BookIds
                        };
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        // Log or handle the exception appropriately
                        throw new Exception("Error adding order. See inner exception for details.", ex);
                    }
                }
            }
        }




        public async Task<List<OrderDetailsResponse>> GetOrderDetails(int userId)
        {
            var queryOrders = "SELECT * FROM Orders WHERE UserId = @UserId";
            var queryBooks = "SELECT b.*, ob.OrderId FROM Books b INNER JOIN OrderBooks ob ON b.BookId = ob.BookId WHERE ob.OrderId IN @OrderIds";
            var queryAddresses = "SELECT * FROM Addresses WHERE AddressId IN @AddressIds";
            var queryUsers = "SELECT * FROM Users WHERE UserId = @UserId";

            using (var connection = _context.CreateConnection())
            {
                var orders = (await connection.QueryAsync<Order>(queryOrders, new { UserId = userId })).ToList();
                if (orders == null || orders.Count == 0) return null;

                var orderIds = orders.Select(o => o.OrderId).ToList();
                var books = (await connection.QueryAsync<BookWithOrderId>(queryBooks, new { OrderIds = orderIds })).ToList();
                var addressIds = orders.Select(o => o.AddressId).ToList();
                var addresses = (await connection.QueryAsync<Address>(queryAddresses, new { AddressIds = addressIds })).ToDictionary(a => a.addressId);
                var user = await connection.QuerySingleOrDefaultAsync<UserEntity>(queryUsers, new { UserId = userId });

                var orderDetailsResponses = orders.Select(order => new OrderDetailsResponse
                {
                    Order = order,
                    Books = books.Where(b => b.OrderId == order.OrderId).ToList(),
                    Address = addresses.ContainsKey(order.AddressId) ? addresses[order.AddressId] : null,
                    User = user
                }).ToList();

                return orderDetailsResponses;
            }
        }
}
}
