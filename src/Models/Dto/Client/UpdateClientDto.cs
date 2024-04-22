using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto.Client
{
	public class UpdateClientDto
	{
		public string? Name { get; set; }
		public IFormFile? Image { get; set; }
		[RegularExpression("^01[0-2|5]{1}[0-9]{8}$")]
		public string? PhoneNumber { get; set; }
	}
}
