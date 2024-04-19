namespace src.Models
{
	public class Project //TODO: change name to Project
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		// TODO: budget
		public DateTime StartDate { get; private set; } = DateTime.Now; // TODO: Change to CreatedDate
		public Duration? ExpectedDuration { get; set; } // TODO: make it set by client on updated

		#region relations
		// TODO: required skills
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
		public required string ClientId {  get; set; }
		public Client? Client { get; set; }
		// Todo: Team realtion ship (every Project has a team and every team has one or more Projects)
		// Todo: Payment reationship
		// Todo: Proposal relationship
		#endregion

	}
	public class Duration
	{
		public int Days { get; set; }
		public int Hours { get; set; }
		public Duration(int days, int hours)
		{
			Days = days;
			Hours = hours;
		}
		public override string ToString()
		{
			return $"{Days} day(s) {Hours} hour(s)";
		}
	}


}
