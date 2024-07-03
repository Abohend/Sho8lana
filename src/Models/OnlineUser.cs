using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class OnlineUser
    {
        [Key]
        public string UserId { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
    }
}
