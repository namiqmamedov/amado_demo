using Amado.Models;

namespace Amado.Interface
{
        public interface IEmailService
        {
            Task SendEmailForNewProduct(UserEmailOptions userEmailOptions);
        }
}
