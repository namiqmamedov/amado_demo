using Amado.ViewModels.Cart;

namespace Amado.Interface
{
    public interface ILayoutServices
    {
        Task<Dictionary<string, string>> GetSettingsAsync();
        Task<List<CartVM>> GetCart();
    }
}
