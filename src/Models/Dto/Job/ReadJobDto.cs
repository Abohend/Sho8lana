namespace src.Models.Dto.Job
{
	public class ReadJobDto
	{
		public int Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int ProjectId { get; set; }
		public string? FreelancerId { get; set; } // mapped manually
	}
}
