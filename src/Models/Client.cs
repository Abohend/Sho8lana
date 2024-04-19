namespace src.Models
{
	public class Client: ApplicationUser
	{
		public List<Project>? Projects { get; set; }
	}
}
