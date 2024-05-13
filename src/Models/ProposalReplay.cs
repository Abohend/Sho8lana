using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace src.Models
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
