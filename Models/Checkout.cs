using System.ComponentModel.DataAnnotations;

namespace Amado.Models
{
    public class Checkout
    {
        public int ID { get; set; }
        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(255)]
        [Required]
        public string LastName { get; set; }
        [StringLength(255)]
        [Required]
        public string CompanyName { get; set; }
        [StringLength(255)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [StringLength(255)]
        [Required]
        public string Address { get; set; }
        [StringLength(255)]
        [Required]
        public string Town { get; set; }
        [StringLength(255)]
        [Required]
        public string Zip { get; set; }
        [StringLength(255)]
        [Phone]
        [Required]
        public string Phone { get; set; }

        [StringLength(4255)]
        [Required]
        public string Comment { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public Nullable<DateTime> CreatedAt { get; set; }
    }
}
