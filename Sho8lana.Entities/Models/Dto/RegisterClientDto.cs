﻿using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto
{
	public class RegisterClientDto
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
		public required string Name { get; set; }
	}
}
