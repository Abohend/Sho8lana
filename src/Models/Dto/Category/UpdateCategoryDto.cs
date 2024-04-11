using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto.Category
{
	public class UpdateCategoryDto
	{
		[MinLength(2)]
		[MaxLength(20)]
		public string? Name { get; set; }
		public IFormFile? Image { get; set; }
	}
}
