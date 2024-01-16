using Amado.ViewModels.Cart;

namespace Amado.ViewModels.Header
{
    public class HeaderVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<CartVM> CartVMs { get; set; }
    }
}
