using System.ComponentModel.DataAnnotations;

namespace Amado.Models
{
    public class Color
    {
        public int ID { get; set; }
        [StringLength(255)]
        [Required]
        public string Name { get; set; }
        //public IEnumerable<ProductColor> ProductColors { get; set; }
    }
}
