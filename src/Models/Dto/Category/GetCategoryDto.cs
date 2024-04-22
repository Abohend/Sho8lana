using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
