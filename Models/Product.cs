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
        [Required]

        public int CategoryID { get; set; }
        public Category Category { get; set; }
        [Required]
        public int BrandID { get; set; }
        public Brand Brand { get; set; }

        public List<ProductDescription> Description { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductColor> ProductColors { get; set; }

        [NotMapped]
        [Required]
        public List<string> ProductDescription { get; set; } = new List<string>();

        [NotMapped]
        [Required]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        [Required]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        [Required]
        [MaxLength(5)]
        public IEnumerable<IFormFile> Files { get; set; }

        [NotMapped]
        public List<int> ColorIDs { get; set; }
        [NotMapped]
        public List<int> Counts{ get; set; }
    }
}
