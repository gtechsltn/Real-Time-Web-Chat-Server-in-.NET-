using RealTimeWebChat.Api.Models;

namespace RealTimeWebChat.Api.Contacts
{
    public interface IChatRepository
    {
        Task<int> AddMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetAllMessagesAsync();
    }
}