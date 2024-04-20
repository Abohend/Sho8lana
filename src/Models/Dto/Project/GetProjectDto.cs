using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using src.Models.Dto.Category;

namespace src.Models.Dto.Project
{
    public class GetProjectDto
    {
        public GetProjectDto(){}
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
		public Duration? ExpectedDuration { get; set; }
		public UserDto? Client { get; set; }
		public GetCategoryDto? Category { get; set; }
		public List<SkillDto>? Skills { get; set; }
		
	}
}
