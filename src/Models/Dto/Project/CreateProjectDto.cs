namespace src.Models.Dto.Project
{
    public class CreateProjectDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int CategoryId { get; set; }

    }
}
