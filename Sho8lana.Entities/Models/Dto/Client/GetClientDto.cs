namespace Sho8lana.Entities.Models.Dto.Client
{
    public class ReadClientDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? Balance { get; set; }
        public List<int>? ProjectsId { get; set; }

    }
}
