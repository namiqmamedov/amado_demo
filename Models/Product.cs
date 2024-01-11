using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
        public int Count { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int BrandID { get; set; }
        public Brand Brand { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        public List<ProductColor> ProductColors { get; set; }

        [NotMapped]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        [MaxLength(5)]
        public IEnumerable<IFormFile> Files { get; set; }

        [NotMapped]
        public List<int> ColorIDs { get; set; }
        [NotMapped]
        public List<int> Counts{ get; set; }
    }
}
