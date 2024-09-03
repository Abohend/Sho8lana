namespace Sho8lana.Entities.Models
{
	public class Category
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string ImagePath { get; set; }
		public List<Freelancer>? Freelancers { get; set; }
		public List<Project>? Projects { get; set; }

	}
}
