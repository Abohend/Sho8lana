namespace Sho8lana.Entities.Models
{
	public class ProjectProposal: Proposal
	{

		#region Relations
		public int ProjectId { get; set; }
		public Project? Project { get; set; }
		#endregion
	
	}
}
