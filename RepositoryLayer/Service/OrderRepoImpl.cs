using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestDto;
using ModelLayer.ResponseDto;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                        var parameters = new DynamicParameters();
                        parameters.Add("AddressId", order.AddressId);
                        parameters.Add("UserId", userId);
                        parameters.Add("OrderDate", DateTime.UtcNow);
                        parameters.Add("OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        await connection.ExecuteAsync(
                            "sp_AddOrder",
                            parameters,
                            transaction: transaction,
                            commandType: CommandType.StoredProcedure
                        );

                        var orderId = parameters.Get<int>("OrderId");

                        foreach (var bookId in order.BookIds)
                        {
                            await connection.ExecuteAsync(
                                "sp_AddOrderBook",
                                new { OrderId = orderId, BookId = bookId },
                                transaction: transaction,
                                commandType: CommandType.StoredProcedure
                            );
                        }

                        transaction.Commit();

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
                        throw new Exception("Error adding order. See inner exception for details.", ex);
                    }
                }
            }
        }

        public async Task<List<OrderDetailsResponse>> GetOrderDetails(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var orders = (await connection.QueryAsync<Order>(
                    "sp_GetOrdersByUserId",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                if (orders == null || orders.Count == 0) return null;

                var orderIds = orders.Select(o => o.OrderId).ToList();
                var addressIds = orders.Select(o => o.AddressId).ToList();

                var books = (await connection.QueryAsync<BookWithOrderId>(
                    "sp_GetBooksByOrderIds",
                    new { OrderIds = orderIds.ToDataTable("dbo.IntList") },
                    commandType: CommandType.StoredProcedure
                )).ToList();

                var addresses = (await connection.QueryAsync<Address>(
                    "sp_GetAddressesByAddressIds",
                    new { AddressIds = addressIds.ToDataTable("dbo.IntList") },
                    commandType: CommandType.StoredProcedure
                )).ToDictionary(a => a.addressId);

                var user = await connection.QuerySingleOrDefaultAsync<UserEntity>(
                    "sp_GetUserByUserId",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );

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

    public static class Extensions
    {
        public static DataTable ToDataTable(this List<int> ids, string typeName)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            ids.ForEach(id => table.Rows.Add(id));
            table.SetTypeName(typeName);
            return table;
        }
    }
}
