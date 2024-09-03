namespace Sho8lana.Entities.Models.Dto.Chat
{
    public class CreateMessageDto
    {
        public string SenderId { get; set; } = string.Empty;
        public MessageType MessageType { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
