namespace src.Models
{
	public class Client: ApplicationUser
	{
		public List<Job>? Jobs { get; set; }
	}
}
