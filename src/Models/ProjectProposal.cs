namespace src.Models
{
	public class ProjectProposal
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; private set; } = DateTime.Now;
		
		//Set by freelancer
		public string? Description { get; set; }
		public decimal OfferedPrice { get; set; }
		public DateOnly OfferedDeliverDate { get; set; }
		
		// set by the client "Project owner"
		public bool? IsAccepted { get; set; } = null;
		public string? ClientNote { get; set; }

		#region Relations
		public string FreelancerId { get; set; } = string.Empty;
		public Freelancer? Freelancer { get; set; }
		
		public int ProjectId { get; set; }
		public Project? Project { get; set; }
		#endregion
	
	}
}
