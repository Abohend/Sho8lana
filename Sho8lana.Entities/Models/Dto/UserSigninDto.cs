using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto
{
	public class UserSigninDto
	{
		[DataType(DataType.EmailAddress)]
		public required string Email {  get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }
	}
}
