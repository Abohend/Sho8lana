namespace src.Models.Dto.JobProposal
{
	public class CreateJobProposalDto
	{
		public decimal Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public string? Description { get; set; }

		// reciever Id
		public string FreelancerId { get; internal set; } = string.Empty;
		public int JobId { get; set; }
	}
}
