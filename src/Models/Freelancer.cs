using System.ComponentModel.DataAnnotations;

namespace src.Models
{
	public class Freelancer: ApplicationUser
	{
		public int ExperienceYears { get; set; }

		#region Relations
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
		
		public List<Skill>? Skills { get; set; }

		public List<ProjectProposal>? ProjectsProposal { get; set; }

		public List<JobProposal>? JobProposals { get; set; }
	
		#endregion
		//[Range(1, 5)]
		//public float LeaderRating { get; set; }
		// TODO: TeammemberRating = 70% client + 30% team members
	}
}
