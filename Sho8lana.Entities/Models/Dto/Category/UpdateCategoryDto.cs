using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto.Category
{
	public class UpdateCategoryDto
	{
		[MinLength(2)]
		[MaxLength(20)]
		public string? Name { get; set; }
		public IFormFile? Image { get; set; }
	}
}
