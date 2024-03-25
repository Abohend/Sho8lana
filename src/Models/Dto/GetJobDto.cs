using System.ComponentModel.DataAnnotations;

namespace src.Models.Dto
{
	public class GetJobDto
	{
        public GetJobDto()
        {
            
        }
        public int Id { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; }
		public DateTime StartDate { get; set; }
		public UserDto? ClientDto { get; set; }
		[Required]
		public CategoryDto? CategoryDto { get; set; }
		
	}
}
