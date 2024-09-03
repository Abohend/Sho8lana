using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models
{
	public class ApplicationUser: IdentityUser
	{
		[MinLength(2)]
		public string? Name { get; set; }
		public string? ImagePath { get; set; }
		public decimal Balance { get; set; } = 0;
		
		public List<GroupChat>? Chats { get; set; }
        public List<Message>? MessagesSent { get; set; }
        public List<Message>? MessagesReceived { get; set; }
    }
}
