namespace src.Models
{
	public class Freelancer: ApplicationUser
	{
		public required int CategoryId { get; set; }
		public Category? Category { get; set; }
	}
}
