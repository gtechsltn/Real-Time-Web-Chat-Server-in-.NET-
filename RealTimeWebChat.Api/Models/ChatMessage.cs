namespace RealTimeWebChat.Api.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
