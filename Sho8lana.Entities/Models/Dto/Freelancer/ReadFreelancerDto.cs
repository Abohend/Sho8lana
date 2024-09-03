namespace Sho8lana.Entities.Models.Dto.Freelancer
{
	public class ReadFreelancerDto
	{
		public string? Id { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? ImageUrl { get; set; }
		public string? PhoneNumber { get; set; }
		public decimal? Balance { get; set; }
		public int? CategoryId { get; set; }
		public List<SkillDto>? Skills { get; set; }
		
		//commented because may freelancer need this data to be private
		//todo "possible solution" : only return jobs and projects he had already completed with only needed data (ex => don't show price that he taken). 
		//public List<ReadJobDto>? Jobs { get; set; }
		//public List<ReadProjectDto>? Projects { get; set; }
	}
}
