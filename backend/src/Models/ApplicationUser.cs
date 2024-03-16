using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace src.Models
{
	public class ApplicationUser: IdentityUser
	{
		[MinLength(2)]
		public string? Name { get; set; }
	}
}
