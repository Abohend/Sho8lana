using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto.Project
{
    public class CreateProjectDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int CategoryId { get; set; }
        public List<int>? RequiredSkillsId { get; set; }
        public Duration? ExpectedDuration { get; set; }
		public decimal? ExpectedBudget { get; set; }
	}
}
