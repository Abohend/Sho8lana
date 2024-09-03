using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models
{
    public class DeliveredProduct
    {
        public string GitHubUrl { get; set; } = string.Empty;
        public bool Verified { get; set; } = false;
    }
}
