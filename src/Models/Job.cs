namespace src.Models
{
	public class Job
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public DateTime StartDate { get; private set; } = DateTime.Now;
		public Duration? ExpectedDuration { get; set; }

		#region relations
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
		public required string ClientId {  get; set; }
		public Client? Client { get; set; }
		// Todo: Team realtion ship (every Job has a team and every team has one or more jobs)
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
