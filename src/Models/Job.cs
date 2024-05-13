namespace src.Models
{
	public class Job
	{
		public int Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		
		public Project? Project { get; set; }
		public int ProjectId { get; set; }

		public List<JobProposal>? Proposals { get; set; }
	}
}
