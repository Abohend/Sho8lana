namespace src.Models.Dto.ProjectProposal
{
	public class CreateProjectProposalDto
	{
		public float OfferedPrice { get; set; }
		public DateOnly OfferedDeliverDate { get; set; }
		public string? Description { get; set; }

		public string FreelancerId { get; internal set; } = string.Empty;
		public int ProjectId { get; set; }
	}
}
