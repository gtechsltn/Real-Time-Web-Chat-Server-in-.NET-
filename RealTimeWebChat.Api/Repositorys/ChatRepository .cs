using Dapper;
using RealTimeWebChat.Api.Contacts;
using RealTimeWebChat.Api.Data;
using RealTimeWebChat.Api.Models;

namespace RealTimeWebChat.Api.Repositorys
{
    public class ChatRepository : IChatRepository
    {
        private readonly DapperContext _connectionString;

        public ChatRepository(DapperContext connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesAsync()
        {
            using var connection = _connectionString.CreateDbConnection();
            return await connection.QueryAsync<ChatMessage>("SELECT * FROM ChatMessages ORDER BY Timestamp");
        }

        public async Task<int> AddMessageAsync(ChatMessage message)
        {
            var sql = "INSERT INTO ChatMessages (Name, Message, Timestamp) VALUES (@Name, @Message, @Timestamp)";
            try
            {
                using var connection = _connectionString.CreateDbConnection();
                return await connection.ExecuteAsync(sql, new { message.Name, message.Message, message.Timestamp });
            }
            catch (Exception ex) 
            {
                return 0;
            }
        }
    }
}
