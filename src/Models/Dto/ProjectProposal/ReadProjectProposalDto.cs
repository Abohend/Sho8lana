namespace src.Models.Dto.ProjectProposal
{
	public class ReadProjectProposalDto
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; private set; } = DateTime.Now;
		public string? Description { get; set; }
		public decimal OfferedPrice { get; set; }
		public DateOnly OfferedDeliverDate { get; set; }
		public bool? IsAccepted { get; set; } = null;
		public string? ClientNote { get; set; }
		public string FreelancerId { get; set; } = string.Empty;
		public int ProjectId { get; set; }
	}
}
