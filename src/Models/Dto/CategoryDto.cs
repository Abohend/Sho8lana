using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto
{
	public class CategoryDto
	{
		[MinLength(2)]
		[MaxLength(20)]
		public required string Name { get; set; }
	}
}
