using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models
{
    public class OnlineUser
    {
        [Key]
        public string UserId { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
    }
}
