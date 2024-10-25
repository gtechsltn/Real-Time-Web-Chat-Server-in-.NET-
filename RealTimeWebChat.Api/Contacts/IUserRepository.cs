using RealTimeWebChat.Api.Models;

namespace RealTimeWebChat.Api.Contacts
{
    public interface IUserRepository
    {
        Task<string> AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserAsync(string username);
    }
}