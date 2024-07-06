using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class DeliveredProduct
    {
        public string GitHubUrl { get; set; } = string.Empty;
        public bool Verified { get; set; } = false;
    }
}
