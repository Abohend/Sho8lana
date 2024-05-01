namespace src.Models.Dto.Freelancer
{
	public class GetFreelancerDto
	{
		public string? Id { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? ImageUrl { get; set; }
		public string? PhoneNumber { get; set; }
		public decimal? Balance { get; set; }
		public int? CategoryId { get; set; }
		public List<int>? ProjectsId { get; set; }
		public List<SkillDto>? Skills { get; set; }
	}
}
