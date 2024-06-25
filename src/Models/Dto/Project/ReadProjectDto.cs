using src.Models.Dto.Category;

namespace src.Models.Dto.Project
{
    public class ReadProjectDto
    {
        public ReadProjectDto(){}
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
		public decimal? ExpectedBudget { get; set; }
		public Duration? ExpectedDuration { get; set; }
		public string? ClientId { get; set; }
		public int CategoryId { get; set; }
		//public GetCategoryDto? Category { get; set; }
		public List<SkillDto>? Skills { get; set; }
		
	}
}
