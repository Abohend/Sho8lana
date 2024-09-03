namespace Sho8lana.Entities.Models.Dto.Proposal
{
	public class CreateProposalDto
	{
		public decimal Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public string? Description { get; set; }
		public string FreelancerId { get; set; } = string.Empty;
		public int WorkId { get; set; } // JobId or ProjectId
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
