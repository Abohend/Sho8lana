namespace src.Models.Dto.JobProposal
{
	public class ReadJobProposalDto
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; private set; } = DateTime.Now;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public bool? IsAccepted { get; set; } = null;
		public string? Note { get; set; }
		public string FreelancerId { get; set; } = string.Empty;
		public int JobId { get; set; }
	}
}
