using System.ComponentModel.DataAnnotations;

namespace src.Models
{
	public class Project 
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		// TODO: budget
		public DateTime CreatedTime { get; private set; } = DateTime.Now; 
		public Duration? ExpectedDuration { get; set; } 

		#region relations
		// TODO: required skills
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
		public required string ClientId {  get; set; }
		public Client? Client { get; set; }
		public string? FreelancerId { get; set; }
		public Freelancer? Freelancer { get; set; }
		public List<Skill>? Skills { get; set; } = new List<Skill>();
		public List<ProjectProposal>? Proposals { get; set; }
		// Todo: Team realtion ship (every Project has a team and every team has one Project)
		// Todo: Payment reationship
		#endregion

	}
	public class Duration
	{
		[Range(0, int.MaxValue)]
		public int Months { get; set; }
		[Range(1, 29)]
		public int Days { get; set; }
		public Duration() { }
		public Duration(int days, int months)
		{
			Days = days;
			Months = months;
		}
		public override string ToString()
		{
			return $"{Months} Month(s) {Days} day(s)";
		}
	}


}
