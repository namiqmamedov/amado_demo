using Amado.DAL;
using Amado.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amado.ViewComponents
{
    public class NewsletterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public NewsletterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Subscriber subscriber)
        {
            return View(await Task.FromResult(subscriber));
        }
    }
}
