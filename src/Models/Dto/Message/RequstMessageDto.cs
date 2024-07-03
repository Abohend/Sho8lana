namespace src.Models.Dto.Message
{
    public class RequstMessageDto
    {
        public string senderId { get; internal set; } = string.Empty;
        public string receiverId { get; set; } = string.Empty;
        public MessageType ChatType { get; set; }
    }
}
