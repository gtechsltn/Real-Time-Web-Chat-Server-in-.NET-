using Dapper;
using RealTimeWebChat.Api.Contacts;
using RealTimeWebChat.Api.Data;
using RealTimeWebChat.Api.Models;

namespace RealTimeWebChat.Api.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _connectionString;

        public UserRepository(DapperContext connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> GetUserAsync(string username)
        {
            const string query = "select *from Users where Name=@Name";

            using var connection = _connectionString.CreateDbConnection();

            return await connection.QueryFirstAsync<User>(query, new { Name= username });
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            using var connection = _connectionString.CreateDbConnection();
            return await connection.QueryAsync<User>("SELECT * FROM Users ORDER BY Name");
        }

        public async Task<string> AddUserAsync(User user)
        {
            var sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
            using var connection = _connectionString.CreateDbConnection();
            var result= await connection.ExecuteAsync(sql, new { user.Name, user.Email });
            if (result > 0) return user.Email;
            else return "";

        }
    }
}
