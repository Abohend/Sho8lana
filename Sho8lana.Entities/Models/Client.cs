namespace Sho8lana.Entities.Models
{
	public class Client: ApplicationUser
	{
		public List<Project>? Projects { get; set; }
	}
}
