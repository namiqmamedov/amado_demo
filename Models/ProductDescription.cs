using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class ProductDescription
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; }
    }
}
