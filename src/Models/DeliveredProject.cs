using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class DeliveredProject : DeliveredProduct
    {
        [Key]
        public int ProjectId { get; set; } 
        public Project? Project { get; set; }
    }
}
