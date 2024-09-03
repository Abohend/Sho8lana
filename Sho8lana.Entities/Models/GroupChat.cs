namespace Sho8lana.Entities.Models
{
    public class GroupChat
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AdminId { get; set; } = string.Empty;

        public ApplicationUser? Admin { get; set; }
        public ICollection<ApplicationUser>? Members { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
