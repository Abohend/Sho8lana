using Microsoft.EntityFrameworkCore;

namespace Sho8lana.Entities.Models
{
	[Keyless]
	public class ProposalReplay
	{
		public bool IsAccepted { get; set; }
		public string? Note { get; set; }
		
		#region Relations
		public Proposal? Proposal {  get; set; }
		public int ProposalId { get; set; }
		#endregion
	}
}
