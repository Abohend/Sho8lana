using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto
{
	public class UserRegisterDto
	{
		[DataType(DataType.EmailAddress)]
		public required string Email { get; set; }

		[DataType(DataType.Password)]
		[MinLength(6)]
		[MaxLength(22)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$")]
		public required string Password { get; set; }
		
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string? ConfirmPassword { get; set; }
		
		public required string AccountType { get; set; }
		public int? CategoryId { get; set; }
		public required string Name { get; set; }
	}

	public enum AccountType
	{
		freelancer,
		client
	}
}
