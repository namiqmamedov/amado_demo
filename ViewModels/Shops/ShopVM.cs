using Amado.Models;

namespace Amado.ViewModels.Shops
{
    public class ShopVM
    {
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public PageNationList<Product> ProductsPageList { get; set; }

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                ProductsPageList = PageNationList<Product>.Create(value.AsQueryable(), 1, 5);
            }
        }
    }
}
