namespace Sho8lana.Entities.Models.Dto.Message
{
    public class RequstMessageDto
    {
        public string senderId { get; set; } = string.Empty;
        public string receiverId { get; set; } = string.Empty;
        public MessageType ChatType { get; set; }
    }
}
