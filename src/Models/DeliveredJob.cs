using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class DeliveredJob : DeliveredProduct 
    {
        [Key]
        public int JobId { get; set; }
        public Job? Job { get; set; }

    }
}
