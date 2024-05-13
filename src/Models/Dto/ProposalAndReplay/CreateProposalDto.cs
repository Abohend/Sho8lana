namespace src.Models.Dto.Proposal
{
	public class CreateProposalDto
	{
		public float Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public string? Description { get; set; }
		public string FreelancerId { get; internal set; } = string.Empty;
		public int WorkId { get; set; } // JobId or ProjectId
	}
}
