namespace Sho8lana.Entities.Models
{
	public class JobProposal : Proposal
	{
		#region
		public int JobId { get; set; }
		public Job? Job { get; set; }
		#endregion
	}
}
