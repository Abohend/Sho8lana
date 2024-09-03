using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto.Category
{
	public class CreateCategoryDto
	{
		[Required]
		[MinLength(2)]
		[MaxLength(20)]
		public required string Name { get; set; }
		[Required]
		public required IFormFile Image { get; set; }
	}
}
