using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models.Dto.Freelancer
{
	public class UpdateFreelancerDto
	{
		public string? Name { get; set; }
		public IFormFile? Image { get; set; }
		[RegularExpression("^01[0-2|5]{1}[0-9]{8}$")]
		public string? PhoneNumber { get; set; }
		[Range(0, 50)]
		public int? ExperienceYears { get; set; }

		public int? CategoryId { get; set; }
		public List<int>? SkillsId { get; set; }
	}
}
