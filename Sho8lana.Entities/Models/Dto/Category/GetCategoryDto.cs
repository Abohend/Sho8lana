using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto.Category
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
