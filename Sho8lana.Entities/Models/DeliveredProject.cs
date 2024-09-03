using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models
{
    public class DeliveredProject : DeliveredProduct
    {
        [Key]
        public int ProjectId { get; set; } 
        public Project? Project { get; set; }
    }
}
