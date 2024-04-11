using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto.Category
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
