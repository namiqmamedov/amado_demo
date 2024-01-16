using Amado.Models;

namespace Amado.Interface
{
    public interface IEmailRepository
    {
        Task GenerateLinkForNewProduct(Product product);
    }
}
