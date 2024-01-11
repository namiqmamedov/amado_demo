
namespace Amado.Models
{
    public class ProductDescription
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string Text { get; set; }
    }
}
