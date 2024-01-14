using Amado.DAL;
using Amado.Models;
using Amado.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amado.Areas.admin.Controllers
{
    [Area("Admin")]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CheckoutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Checkout> checkouts = _context.Checkouts.OrderBy(x => x.ID);

            return View(PageNationList<Checkout>.Create(checkouts, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Checkout checkout = await _context.Checkouts.FirstOrDefaultAsync(x => x.ID == ID);

            if (checkout == null) return NotFound();

            return View(checkout);
        }
    }
}
