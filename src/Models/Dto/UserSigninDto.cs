using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto
{
	public class UserSigninDto
	{
		[DataType(DataType.EmailAddress)]
		public required string Email {  get; set; }

		[DataType(DataType.Password)]
		public required string Password { get; set; }
	}
}
