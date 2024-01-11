using Amado.Models;

namespace Amado.ViewModels.Products
{
    public class ProductVM
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public ProductColor ProductColor { get; set; }
    }
}
