namespace Sho8lana.Entities.Models
{
	public class Proposal
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		public decimal Price { get; set; }
		public string Description { get; set; } = string.Empty;
		public DateOnly DeliverDate { get; set; }

		#region Relations
		public string FreelancerId { get; set; } = string.Empty;
		public Freelancer? Freelancer { get; set; }

		public ProposalReplay? ProposalReplay { get; set; }
		#endregion
	}
}
