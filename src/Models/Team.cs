using System.ComponentModel.DataAnnotations;

namespace src.Models
{
	public class Team
	{
		[Key]
		public int ProjectId { get; set; }
		public List<string?>? MembersId { get; set; }
	}
}
