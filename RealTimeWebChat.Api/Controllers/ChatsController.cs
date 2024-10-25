using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeWebChat.Api.Contacts;
using RealTimeWebChat.Api.Hubs;
using RealTimeWebChat.Api.Models;
using RealTimeWebChat.Api.Repositorys;

namespace RealTimeWebChat.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChatsController(IChatRepository chatRepository, IUserRepository userRepository, IHubContext<ChatHub> chatHubContext)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _chatHubContext = chatHubContext;
        }


        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("user content cannot be empty.");
            }

            var userId = await _userRepository.AddUserAsync(user);

            return Ok(new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _chatRepository.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] ChatMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message))
            {
                return BadRequest("Message content cannot be empty.");
            }
            User userInfo=await _userRepository.GetUserAsync(message.Name);

            if (userInfo != null) {
                if (userInfo.Name!= message.Name) return BadRequest("User is not valid");
            }

            var messageId = await _chatRepository.AddMessageAsync(message);
            await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", message.Name, message.Message);

            return Ok(new { Id = messageId });
        }
    }
}
