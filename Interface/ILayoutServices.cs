using Amado.ViewModels.Cart;

namespace Amado.Interface
{
    public interface ILayoutServices
    {
        Task<List<CartVM>> GetCart();
    }
}
