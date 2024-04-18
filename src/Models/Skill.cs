namespace src.Models
{
	public class Skill
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public List<Freelancer>? Freelancers { get; set; }
	}


	/// freelacer -----------<>----------- Skill
}
