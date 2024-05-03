using System.ComponentModel.DataAnnotations;

namespace src.Models
{
	public class Freelancer: ApplicationUser
	{
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
		public List<Skill>? Skills { get; set; }
		[Range(0, 50)]
		public int ExperienceYears { get; set; }
		//[Range(1, 5)]
		//public float LeaderRating { get; set; }
		// TODO: TeammemberRating = 70% client + 30% team members
		public List<Project>? Projects { get; set; }

		public List<Job>? Jobs { get; set; }

	}
}
