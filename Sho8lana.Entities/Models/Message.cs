namespace Sho8lana.Entities.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SenderId { get; set; } = string.Empty;
        public ApplicationUser? Sender { get; set; }
        public MessageType MessageType { get; set; }
        public string? ReceiverUserId { get; set; }
        public ApplicationUser? ReceiverUser { get; set; }
        public string? ReceiverGroupId { get; set; }
        public GroupChat? ReceiverGroup { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public enum MessageType
    {
        Individual,
        Group
    }

}
