using Amado.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Amado.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(IDictionary<string, string> setting)
        {
            return View(await Task.FromResult(setting));
        }
    }
}
