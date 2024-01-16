using System.ComponentModel.DataAnnotations;

namespace Amado.Models
{
    public class Subscriber
    {
        public int ID { get; set; }
        
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
