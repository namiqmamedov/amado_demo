using Amado.Models;
using Amado.ViewModels.Cart;

namespace Amado.ViewModels.Newsletter
{
    public class NewsletterVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public Subscriber Subscriber { get; set; }
    }
}
