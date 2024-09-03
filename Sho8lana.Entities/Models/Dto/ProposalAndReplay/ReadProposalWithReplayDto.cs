namespace Sho8lana.Entities.Models.Dto.ProposalAndReplay
{
	public class ReadProposalWithReplayDto
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; private set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public DateOnly DeliverDate { get; set; }
		public ProposalReplayDto? ProposalReplay { get; set; }
		public string FreelancerId { get; set; } = string.Empty;
		public int WorkId { get; set; }
	}
}
