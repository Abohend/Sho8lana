namespace src.Models.Dto.Proposal
{
	public class CreateProposalDto
	{
		public decimal Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public string? Description { get; set; }
		public string FreelancerId { get; internal set; } = string.Empty;
		public int WorkId { get; internal set; } // JobId or ProjectId
	}

	public class CreateJobProposalDto : CreateProposalDto
	{
		public new string FreelancerId 
		{
			get { return base.FreelancerId; }
			set { base.FreelancerId = value;}
		}
	}
}
