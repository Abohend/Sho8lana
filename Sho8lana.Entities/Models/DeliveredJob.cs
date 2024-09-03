using System.ComponentModel.DataAnnotations;

namespace Sho8lana.Entities.Models
{
    public class DeliveredJob : DeliveredProduct 
    {
        [Key]
        public int JobId { get; set; }
        public Job? Job { get; set; }

    }
}
