using Amado.Models;
using Amado.ViewModels.Cart;

namespace Amado.ViewModels.Shops
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<CartVM> CartVMs { get; set; }

    }
}
