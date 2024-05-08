namespace src.Models
{
	public class JobProposal
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; set; }

		// set by leader
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public DateOnly DeliverDate {get; set;}

		// set by freelancer
		public bool? IsAccepted { get; set; } = null;
		public string? Note { get; set; }

		#region
		// sender => Job.Project.FreelancerId

		// reciever
		public string FreelancerId { get; set; } = string.Empty;
		public Freelancer? Freelancer { get; set; }

		public int JobId { get; set; }
		public Job? Job { get; set; }
		#endregion
	}
}
