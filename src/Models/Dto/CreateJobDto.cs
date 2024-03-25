namespace src.Models.Dto
{
	public class CreateJobDto
	{
		public required string Title { get; set; }
		public string? Description { get; set; }
		public required int CategoryId { get; set; }

	}
}
