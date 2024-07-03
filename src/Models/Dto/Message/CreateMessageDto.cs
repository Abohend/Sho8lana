namespace src.Models.Dto.Chat
{
    public class CreateMessageDto
    {
        public string SenderId { get; internal set; } = string.Empty;
        public MessageType MessageType { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
