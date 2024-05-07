using Dapper;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class BookRepoImpl : IBookRepository
    {
        private readonly DapperContext _context;

        public BookRepoImpl(DapperContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBook(Book book)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Title", book.Title);
            parameters.Add("@Author", book.Author);
            parameters.Add("@Price", book.Price);
            parameters.Add("@Description", book.Description);
            parameters.Add("@ImagePath", book.ImagePath);
            parameters.Add("@Quantity",book.Quantity);

            var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync("sp_AddBook", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? book : null;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var connection = _context.CreateConnection();
            return await connection.QueryAsync<Book>("sp_GetAllBooks", commandType: CommandType.StoredProcedure);
        }


        public async Task<Book> DeleteBook(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync("sp_DeleteBook", parameters, commandType: CommandType.StoredProcedure);

            return result > 0 ? new Book { BookId = id } : null;
        }

        public async Task<Book> GetBookById(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Books WHERE BookId = @Id";
                return await connection.QueryFirstOrDefaultAsync<Book>(query, parameters);
            }
        }
    }
}
